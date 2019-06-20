using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Security.Claims;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System.Linq;

namespace OAuthHandler.Pocket
{
    public class PocketHandler : OAuthHandler<PocketOptions>
    {
        public PocketHandler(IOptionsMonitor<PocketOptions> options, ILoggerFactory logger, UrlEncoder encoder, ISystemClock clock)
            : base(options, logger, encoder, clock)
        {

        }
        protected async override Task HandleChallengeAsync(AuthenticationProperties properties)
        {
            var res = await GetRequestTokenAsync();
            if(res.IsSuccess && res.RequestToken != null)
            {
                properties.SetParameter("RequestToken", res.RequestToken.code);
                var cookieOptions = Options.CorrelationCookie.Build(Context, DateTime.Now);
                Response.Cookies.Append("RequestToken", res.RequestToken.code, cookieOptions);
                if(properties.RedirectUri != null)
                {
                    Response.Cookies.Append("RedirectUri", properties.RedirectUri, cookieOptions);
                }
                await base.HandleChallengeAsync(properties);
            }
        }

        /// <summary>
        /// Get Request Token
        /// </summary>
        /// <returns></returns>
        private async Task<PocketRequestTokenResult> GetRequestTokenAsync()
        {
            try
            {
                var requestBody = new PocketRequestTokenRequest() { ConsumerKey = Options.ConsumerKey, RedirectUri = BuildRedirectUri(Options.CallbackPath.Value) };
                var playLoad = PrepareRequestTokenPlayLoad(requestBody);
                var res = await Options.Backchannel.PostAsync(Options.RequestTokenEndpoint, playLoad);
                if(res.IsSuccessStatusCode)
                {
                    var str = await res.Content.ReadAsStringAsync();
                    var tokenRes = JsonConvert.DeserializeObject<PocketRequestTokenResponse>(str);
                    return PocketRequestTokenResult.Success(tokenRes);
                }
                else
                {
                    await ProcessPocketRemoteFail(res);
                }
            }
            catch(Exception ex)
            {
                var remoteFailure = new RemoteFailureContext(Context, Scheme, Options, ex);
                await Events.OnRemoteFailure(remoteFailure);
            }
            return PocketRequestTokenResult.Fail();
        }
        private HttpContent PrepareRequestTokenPlayLoad(object content)
        {
            var playLoad = new StringContent(JsonConvert.SerializeObject(content));
            playLoad.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json")
            {
                CharSet = "UTF-8"
            };
            playLoad.Headers.TryAddWithoutValidation("X-Accept", "application/json");
            return playLoad;
        }
        protected override string BuildChallengeUrl(AuthenticationProperties properties, string redirectUri)
        {
            var queryStrings = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
            queryStrings.Add("request_token",properties.GetParameter<string>("RequestToken"));
            queryStrings.Add("redirect_uri",BuildRedirectUri(Options.CallbackPath.Value));
            return QueryHelpers.AddQueryString(Options.AuthorizationEndpoint, queryStrings);
        }
        protected async override Task<HandleRequestResult> HandleRemoteAuthenticateAsync()
        {
            if (Context.Request.Cookies.TryGetValue("RequestToken", out var requestToken))
            {
                var res = await GetAccessTokenAsync(requestToken);
                if(res.IsSuccess && res.PocketAccessTokenRespons != null)
                {
                    var ticket = CreatePocketTicketAsync(res.PocketAccessTokenRespons);
                    if(Context.Request.Cookies.TryGetValue("RedirectUri",out var redirectUri))
                    {
                        ticket.Properties.RedirectUri = redirectUri;
                    }
                    return HandleRequestResult.Success(ticket);
                }
            }
            return HandleRequestResult.Fail(new ArgumentNullException("RequestToken"));
        }
        private AuthenticationTicket CreatePocketTicketAsync(PocketAccessTokenResponse res)
        {
            var pocketIdentity = new ClaimsIdentity("OAuth");
            pocketIdentity.AddClaim(new Claim(ClaimTypes.NameIdentifier, res.UserName));
            pocketIdentity.AddClaim(new Claim("AccessToken", res.AccessToken));
            var user = new ClaimsPrincipal(pocketIdentity);
            AuthenticationProperties pocketProperties = new AuthenticationProperties();
            pocketProperties.AllowRefresh = true;
            return new AuthenticationTicket(user, pocketProperties, "pocket");
        }
        private async Task<PocketRemoteException> ProcessPocketRemoteFail(HttpResponseMessage res)
        {
            PocketRemoteException pocketRemoteException = new PocketRemoteException(res.StatusCode);
            if (res.Headers.TryGetValues("X-Error-Code", out var xErrorCode))
            {
                pocketRemoteException.XErrorCode = xErrorCode.Aggregate("", (pre, next) => pre + next);
            }
            if (res.Headers.TryGetValues("X-Error", out var xError))
            {
                pocketRemoteException.XError = xErrorCode.Aggregate("", (pre, next) => pre + next);
            }
            var remoteFailure = new RemoteFailureContext(Context, Scheme, Options, pocketRemoteException);
            await Events.OnRemoteFailure(remoteFailure);
            return pocketRemoteException;
        }
        /// <summary>
        /// 根据RquestToken获取AccessToken和UserName
        /// </summary>
        /// <param name="requestToken"></param>
        /// <returns></returns>
        private async Task<PocketAccessTokenResult> GetAccessTokenAsync(string requestToken)
        {
            try
            {
                var requestBody = new PocketAccessTokenRequest() { ConsumerKey = Options.ConsumerKey, Code = requestToken };
                var playLoad = PrepareRequestTokenPlayLoad(requestBody);
                var res = await Options.Backchannel.PostAsync(Options.TokenEndpoint, playLoad);
                if(res.IsSuccessStatusCode)
                {
                    var str = await res.Content.ReadAsStringAsync();
                    var tokenRes = JsonConvert.DeserializeObject<PocketAccessTokenResponse>(str);
                    return PocketAccessTokenResult.Success(tokenRes);
                }
                else
                {
                    var ex = await ProcessPocketRemoteFail(res);
                    return PocketAccessTokenResult.Fail(ex);
                }
            }
            catch(Exception ex)
            {
                return PocketAccessTokenResult.Fail(ex);
            }
        }
    }
}
