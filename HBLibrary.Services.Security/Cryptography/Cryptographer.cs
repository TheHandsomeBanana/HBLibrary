using HBLibrary.Services.Security.Cryptography.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HBLibrary.Services.Security.Cryptography;
public class Cryptographer : ICryptographer {
    public byte[] Decrypt(byte[] data, CryptographySettings? settings = null) {
        throw new NotImplementedException();
    }

    public byte[] Encrypt(byte[] data, CryptographySettings? settings = null) {
        throw new NotImplementedException();
    }
}
