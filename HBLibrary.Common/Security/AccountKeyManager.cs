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
            byte[] keyBuffer;
#if NET5_0_OR_GREATER
            keyBuffer = await File.ReadAllBytesAsync(keyfile);
#elif NET472_OR_GREATER
            using(FileStream fs = new FileStream(keyfile, FileMode.Open, FileAccess.Read)) {
                keyBuffer = await fs.ReadAsync();
            }
#endif
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

    public async Task<Result<RsaKey>> GetPrivateKeyAsync(string identifier, byte[] salt) {
        string keyfile = Path.Combine(GlobalEnvironment.IdentityPath, $"{identifier}.privkey");

        try {
            AesKey aesKey = KeyGenerator.GenerateAesKey(keyfile, salt);

            byte[] protectedKeys;
#if NET5_0_OR_GREATER
            protectedKeys = await File.ReadAllBytesAsync(keyfile);
#elif NET472_OR_GREATER
            using(FileStream fs = new FileStream(keyfile, FileMode.Open, FileAccess.Read)) {
                protectedKeys = await fs.ReadAsync();
            }
#endif

            byte[] privateKeyBuffer = await new AesCryptographer().DecryptAsync(protectedKeys, aesKey);
            if (privateKeyBuffer.Length == 0) {
                return new InvalidOperationException($"Keyfile {keyfile} is empty.");
            }

            JsonSerializerOptions jsonOptions = new JsonSerializerOptions();
            jsonOptions.Converters.Add(new RsaKeyConverter());

            RsaKey? privateKey = JsonSerializer.Deserialize<RsaKey>(privateKeyBuffer, jsonOptions);
            if (privateKey is not null) {
                return privateKey;
            }

            return new InvalidOperationException($"Keyfile {keyfile} is corrupted.");
        }
        catch (Exception ex) {
            return ex;
        }
    }

    public async Task<Result<RsaKeyPair>> CreateAccountKeysAsync(string identifier, byte[] salt) {
        string publicKeyFile = Path.Combine(GlobalEnvironment.IdentityPath, $"{identifier}.pubkey");
        string privateKeyFile = Path.Combine(GlobalEnvironment.IdentityPath, $"{identifier}.privkey");

        if (File.Exists(publicKeyFile) && File.Exists(privateKeyFile)) {
            return new InvalidOperationException($"{identifier} keys are already defined");
        }

        try {
            AesKey aesKey = KeyGenerator.GenerateAesKey(identifier, salt);

            RsaKeyPair keyPair = KeyGenerator.GenerateRsaKeys();

            JsonSerializerOptions jsonOptions = new JsonSerializerOptions();
            jsonOptions.Converters.Add(new RsaKeyConverter());

            byte[] serializedPrivateKey = GlobalEnvironment.Encoding.GetBytes(JsonSerializer.Serialize(keyPair.PrivateKey, jsonOptions));
            byte[] protectedKey = await new AesCryptographer().EncryptAsync(serializedPrivateKey, aesKey);

            using (FileStream fs = File.Create(privateKeyFile)) {
                await fs.WriteAsync(protectedKey);
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