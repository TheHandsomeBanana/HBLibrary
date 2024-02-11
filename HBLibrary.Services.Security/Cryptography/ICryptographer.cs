using HBLibrary.Services.Security.Cryptography.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HBLibrary.Services.Security.Cryptography;
public interface ICryptographer {
    byte[] Encrypt(byte[] data, CryptographySettings? settings = null);
    byte[] Decrypt(byte[] data, CryptographySettings? settings = null);
}
