﻿using System.Security.Cryptography;

namespace HBLibrary.NetFramework.Services.Security.DataProtection {
    public interface IDataProtectionService {
        void SetEntropy(byte[] entropy);
        void SetScope(DataProtectionScope scope);
        byte[] Protect(byte[] data);
        byte[] Unprotect(byte[] data);
    }
}
