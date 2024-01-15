using System;

namespace HBLibrary.NetFramework.Services.Security.Exceptions {
    public class CryptoServiceException : Exception {
        public CryptoServiceException(string message) : base(message) {
        }

        public CryptoServiceException(string message, Exception innerException) : base(message, innerException) {
        }
    }
}
