using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace OAuthHandler.Pocket
{
    public class PocketRequestTokenRequest
    {
        [JsonProperty("consumer_key")]
        public string ConsumerKey { get; set; }
        [JsonProperty("redirect_uri")]
        public string RedirectUri { get; set; }
        [JsonProperty("state")]
        public string State { get; set; }
    }
}
