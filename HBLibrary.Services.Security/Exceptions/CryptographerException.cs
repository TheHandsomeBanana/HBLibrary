using System;

namespace HBLibrary.Services.Security.Exceptions {
    public class CryptographerException : Exception {
        public CryptographerException(string message) : base(message) {
        }

        public CryptographerException(string message, Exception innerException) : base(message, innerException) {
        }

        public static void ThrowIncorrectKey(string key) {
            throw new CryptographerException($"{key} is not valid for this operation.");
        }
    }
}
