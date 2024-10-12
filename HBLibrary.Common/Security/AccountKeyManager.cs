using HBLibrary.Common.Extensions;
using HBLibrary.Common.Security.Aes;
using HBLibrary.Common.Security.Exceptions;
using HBLibrary.Common.Security.Keys;
using HBLibrary.Services.Security.Cryptography.Rsa;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Versioning;
using System.Security;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace HBLibrary.Common.Security;

public class AccountKeyManager {
    public AccountKeyManager() { }

    public bool KeyPairExists(string identifier) {
        string publicKeyfile = Path.Combine(GlobalEnvironment.IdentityPath, $"{identifier}.pubkey");
        string privateKeyfile = Path.Combine(GlobalEnvironment.IdentityPath, $"{identifier}.privkey");

        return File.Exists(publicKeyfile) && File.Exists(privateKeyfile);
    }

    public async Task<Result<RsaKey>> GetPublicKeyAsync(string identifier) {
        string keyfile = Path.Combine(GlobalEnvironment.IdentityPath, $"{identifier}.pubkey");

        if (!File.Exists(keyfile)) {
            return new InvalidOperationException($"Key files for identifier {identifier} do not exist.");
        }

        try {
            byte[] keyBuffer = await UnifiedFile.ReadAllBytesAsync(keyfile);

            JsonSerializerOptions jsonOptions = new JsonSerializerOptions();
            jsonOptions.Converters.Add(new RsaKeyConverter());

            RsaKey? key = JsonSerializer.Deserialize<RsaKey>(keyBuffer, jsonOptions);

            if (key is null) {
                return new InvalidOperationException($"Public key for {identifier} is corrupted.");
            }

            return key;
        }
        catch (Exception ex) {
            return ex;
        }
    }

    public async Task<Result<RsaKey>> GetPrivateKeyAsync(string identifier, SecureString password, byte[] salt) {
        string keyfile = Path.Combine(GlobalEnvironment.IdentityPath, $"{identifier}.privkey");

        try {
            AesKey aesKey = KeyGenerator.GenerateAesKey(password, salt);

            byte[] protectedPrivateKey = await UnifiedFile.ReadAllBytesAsync(keyfile);

            byte[] privateKeyBuffer = new AesCryptographer().Decrypt(protectedPrivateKey, aesKey);
            if (privateKeyBuffer.Length == 0) {
                return new InvalidOperationException($"Keyfile {keyfile} is empty.");
            }

            JsonSerializerOptions jsonOptions = new JsonSerializerOptions();
            jsonOptions.Converters.Add(new RsaKeyConverter());

            string keyString = GlobalEnvironment.Encoding.GetString(privateKeyBuffer);

            RsaKey? privateKey = JsonSerializer.Deserialize<RsaKey>(keyString, jsonOptions);
            if (privateKey is not null) {
                return privateKey;
            }

            return new InvalidOperationException($"Keyfile {keyfile} is corrupted.");
        }
        catch (Exception ex) {
            return ex;
        }
    }
    
    public Result<RsaKey> GetPrivateKey(string identifier, SecureString password, byte[] salt) {
        string keyfile = Path.Combine(GlobalEnvironment.IdentityPath, $"{identifier}.privkey");

        try {
            AesKey aesKey = KeyGenerator.GenerateAesKey(password, salt);

            byte[] protectedPrivateKey = File.ReadAllBytes(keyfile);

            byte[] privateKeyBuffer = new AesCryptographer().Decrypt(protectedPrivateKey, aesKey);
            if (privateKeyBuffer.Length == 0) {
                return new InvalidOperationException($"Keyfile {keyfile} is empty.");
            }

            JsonSerializerOptions jsonOptions = new JsonSerializerOptions();
            jsonOptions.Converters.Add(new RsaKeyConverter());

            string keyString = GlobalEnvironment.Encoding.GetString(privateKeyBuffer);

            RsaKey? privateKey = JsonSerializer.Deserialize<RsaKey>(keyString, jsonOptions);
            if (privateKey is not null) {
                return privateKey;
            }

            return new InvalidOperationException($"Keyfile {keyfile} is corrupted.");
        }
        catch (Exception ex) {
            return ex;
        }
    }

    public async Task<Result<RsaKeyPair>> CreateAccountKeysAsync(string identifier, SecureString password, byte[] salt) {
        string publicKeyFile = Path.Combine(GlobalEnvironment.IdentityPath, $"{identifier}.pubkey");
        string privateKeyFile = Path.Combine(GlobalEnvironment.IdentityPath, $"{identifier}.privkey");

        try {
            AesKey aesKey = KeyGenerator.GenerateAesKey(password, salt);

            RsaKeyPair keyPair = KeyGenerator.GenerateRsaKeys();

            JsonSerializerOptions jsonOptions = new JsonSerializerOptions();
            jsonOptions.Converters.Add(new RsaKeyConverter());

            byte[] serializedPrivateKey = GlobalEnvironment.Encoding.GetBytes(JsonSerializer.Serialize(keyPair.PrivateKey, jsonOptions));
            byte[] protectedKey = new AesCryptographer().Encrypt(serializedPrivateKey, aesKey);

            using (FileStream fs = File.Create(privateKeyFile)) {
                fs.Write(protectedKey);
            }

            string serializedPublicKey = JsonSerializer.Serialize(keyPair.PublicKey, jsonOptions);
            using (FileStream fs = File.Create(publicKeyFile)) {
                await fs.WriteAsync(GlobalEnvironment.Encoding.GetBytes(serializedPublicKey));
            }

            return Result<RsaKeyPair>.Ok(keyPair);
        }
        catch (Exception ex) {
            return ex;
        }
    }
}