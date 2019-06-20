using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;

namespace Sample.Authentications
{
    public class SampleAutenticationHandler : IAuthenticationHandler
    {
        private HttpContext _context;
        public SampleAutenticationHandler()
        {

        }

        public Task<AuthenticateResult> AuthenticateAsync()
        {
            var identity = new ClaimsIdentity("Sample");
            identity.AddClaim(new Claim(ClaimTypes.Name, "tim lv"));
            var user = new ClaimsPrincipal(identity);
            var tiket = new AuthenticationTicket(user,null,"sample");
            return Task.FromResult(AuthenticateResult.Success(tiket));
        }

        public Task ChallengeAsync(AuthenticationProperties properties)
        {
            return Task.CompletedTask;
        }

        public Task ForbidAsync(AuthenticationProperties properties)
        {
            return Task.CompletedTask;
        }

        public Task InitializeAsync(AuthenticationScheme scheme, HttpContext context)
        {
            _context = context;
            return Task.CompletedTask;
        }
    }
}
