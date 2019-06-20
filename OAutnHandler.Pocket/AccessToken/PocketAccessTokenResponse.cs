using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace OAuthHandler.Pocket
{
    public class PocketAccessTokenResponse
    {
        [JsonProperty("access_token")]
        public string AccessToken { get; set; }
        [JsonProperty("username")]
        public string UserName { get; set; }
        [JsonProperty("state")]
        public string State { get; set; }
    }
}
