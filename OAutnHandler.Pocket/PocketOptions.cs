using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.OAuth;

namespace OAuthHandler.Pocket
{
    public class PocketOptions : OAuthOptions
    {
        public string ConsumerKey { get; set; }
        public string RequestTokenEndpoint { get; set; }
        public override void Validate()
        {

            if (string.IsNullOrEmpty(AuthorizationEndpoint))
            {
                throw new ArgumentException(nameof(AuthorizationEndpoint));
            }

            if (string.IsNullOrEmpty(TokenEndpoint))
            {
                throw new ArgumentException(nameof(TokenEndpoint));
            }
            if (string.IsNullOrEmpty(RequestTokenEndpoint))
            {
                throw new ArgumentException(nameof(RequestTokenEndpoint));
            }

            if (!CallbackPath.HasValue)
            {
                throw new ArgumentException(nameof(CallbackPath));
            }
        }
    }
}
