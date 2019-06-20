using System;
using System.Collections.Generic;
using System.Text;

namespace OAuthHandler.Pocket
{
    internal class PocketAccessTokenResult
    {
        public static PocketAccessTokenResult Success(PocketAccessTokenResponse requestToken)
        {
            return new PocketAccessTokenResult(true, requestToken,null);
        }
        public static PocketAccessTokenResult Fail(Exception exception)
        {
            return new PocketAccessTokenResult(false, null,exception);
        }
        public PocketAccessTokenResult(bool isSucess, PocketAccessTokenResponse pocketAccessTokenResponse,Exception exception)
        {
            IsSuccess = isSucess;
            PocketAccessTokenRespons = pocketAccessTokenResponse;
            Exception = exception;
        }

        public PocketAccessTokenResponse PocketAccessTokenRespons { get; private set; }
        public Exception Exception { get; private set; }
        public bool IsSuccess { get; private set; }
    }
}
