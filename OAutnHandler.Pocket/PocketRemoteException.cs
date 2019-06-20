using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace OAuthHandler.Pocket
{
    public class PocketRemoteException : Exception
    {
        public HttpStatusCode HttpStatusCode { get; set; }
        public string XErrorCode { get; set; }
        public string XError { get; set; }
        public PocketRemoteException(HttpStatusCode httpStatusCode) : this(httpStatusCode, null, null)
        {

        }
        public PocketRemoteException(HttpStatusCode httpStatusCode, string xErrorCode, string xError)
        {
            HttpStatusCode = HttpStatusCode;
            XErrorCode = xErrorCode;
            XError = xError;
        }
    }
}
