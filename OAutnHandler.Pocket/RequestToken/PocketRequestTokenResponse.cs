using System;
using System.Collections.Generic;
using System.Text;

namespace OAuthHandler.Pocket
{
    public class PocketRequestTokenResponse
    {
        public string code { get; set; }
        public string state { get; set; }
    }
}
