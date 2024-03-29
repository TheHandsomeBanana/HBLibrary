﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace HBLibrary.Services.Security.Cryptography.Rsa;
#if NET472_OR_GREATER
internal static class RsaKeyHelper {
    // Converts RSAParameters to a byte array via XML serialization
    public static byte[] ToByteArray(this RSA rsa, bool includePrivateParameters) {
        string xmlKey = rsa.ToXmlString(includePrivateParameters);
        return Encoding.UTF8.GetBytes(xmlKey);
    }

    // Initializes an RSACryptoServiceProvider from a byte array containing an XML-serialized key
    public static void FromByteArray(this RSA rsa, byte[] keyData) {
        string xmlKey = Encoding.UTF8.GetString(keyData);
        rsa.FromXmlString(xmlKey);
    }
}
#endif