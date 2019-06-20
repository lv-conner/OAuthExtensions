using System;
using System.Collections.Generic;
using System.Text;

namespace OAuthHandler.Pocket
{
    internal class PocketRequestTokenResult
    {
        public static PocketRequestTokenResult Success(PocketRequestTokenResponse requestToken)
        {
            return new PocketRequestTokenResult(true, requestToken);
        }
        public static PocketRequestTokenResult Fail()
        {
            return new PocketRequestTokenResult(false, null);
        }
        public PocketRequestTokenResult(bool isSucess, PocketRequestTokenResponse requestToken)
        {
            IsSuccess = isSucess;
            RequestToken = requestToken;
        }

        public PocketRequestTokenResponse RequestToken { get; private set; }
        public bool IsSuccess { get; private set; }
    }
}
