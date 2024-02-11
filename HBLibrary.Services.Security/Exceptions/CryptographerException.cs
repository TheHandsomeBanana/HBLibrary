using System;

namespace HBLibrary.Services.Security.Exceptions {
    public class CryptographerException : Exception {
        public CryptographerException(string message) : base(message) {
        }

        public CryptographerException(string message, Exception innerException) : base(message, innerException) {
        }
    }
}
