using HBLibrary.Common.Security;
using HBLibrary.Common.Security.Aes;
using HBLibrary.Common.Security.Keys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using HBLibrary.Common.Extensions;
using PEFile;
using HBLibrary.Services.Security.Cryptography.Rsa;

namespace HBLibrary.Common.Tests;
[TestClass]
public class CryptographerTests {
    [TestMethod]
    public async Task Aes_EncryptDecrypt_RsaKeys() {
        AesKey aesKey = KeyGenerator.GenerateAesKey("password", [1, 2, 3, 4]);
        RsaKeyPair keyPair = KeyGenerator.GenerateRsaKeys();

        JsonSerializerOptions jsonOptions = new JsonSerializerOptions();
        jsonOptions.Converters.Add(new RsaKeyConverter());


        byte[] serializedPrivateKey = GlobalEnvironment.Encoding.GetBytes(JsonSerializer.Serialize(keyPair.PrivateKey, jsonOptions));
        byte[] protectedKey = new AesCryptographer().Encrypt(serializedPrivateKey, aesKey);

        using (FileStream fs = File.Create("rsakey")) {
            fs.Write(protectedKey);
        }

        byte[] protectedPrivateKey = await UnifiedFile.ReadAllBytesAsync("rsakey");

        byte[] privateKeyBuffer = new AesCryptographer().Decrypt(protectedPrivateKey, aesKey);
        
        string privateKeyString = GlobalEnvironment.Encoding.GetString(privateKeyBuffer);

        Assert.AreEqual(serializedPrivateKey.Length, privateKeyBuffer.Length);
        for(int i = 0; i < serializedPrivateKey.Length; i++) {
            Assert.AreEqual(serializedPrivateKey[i], privateKeyBuffer[i]);
        }
    }
}
