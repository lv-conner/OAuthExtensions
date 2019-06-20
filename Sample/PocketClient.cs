using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using OAuthHandler.Pocket;

namespace Sample
{
    public class PocketClient
    {
        private readonly HttpClient _httpClient;
        private readonly HttpContext _httpContext;
        public PocketClient(HttpClient httpClient,IOptionsMonitor<PocketOptions> options,IHttpContextAccessor httpContextAccessor)
        {
            _httpClient = httpClient;
            _httpContext = httpContextAccessor.HttpContext;
        }
    }
}
