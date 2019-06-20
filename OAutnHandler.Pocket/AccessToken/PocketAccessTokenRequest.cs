using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace OAuthHandler.Pocket
{
    public class PocketAccessTokenRequest
    {
        [JsonProperty("consumer_key")]
        public string ConsumerKey { get; set; }
        [JsonProperty("code")]
        public string Code { get; set; }
    }
}
