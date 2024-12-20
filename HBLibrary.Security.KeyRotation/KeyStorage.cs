using HBLibrary.Core;
using HBLibrary.Core.Extensions;
using HBLibrary.DataStructures;
using HBLibrary.Interface.Security.KeyRotation;
using HBLibrary.Interface.Security.Keys;
using HBLibrary.Security.Aes;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace HBLibrary.Security.KeyRotation;
public sealed class KeyStorage : IKeyStorage {
    private readonly MasterKey masterKey;
    public string KeyFile { get; }

    public KeyStorage(string keyFile, MasterKey masterKey) {
        this.KeyFile = keyFile;
        this.masterKey = masterKey;
    }

    public async Task<Result> AddFilesToKeyMap(string keyId, IKey key, string[] files) {

    }

    public async Task<Result> SaveKeyAsync(string keyId, IKey key) {
        try {
            Result<EncryptedKeyFileContent> ekfcRes = await GetEncryptedKeyFileContentAsync();
            if (ekfcRes.IsFaulted) {
                return ekfcRes.PullError();
            }

            EncryptedKeyFileContent ekfc = ekfcRes.Value!;

            Result<string> encryptedValueResult = GetEncryptedValueForKeyIdAsync(keyId, ekfc);

            Result res = encryptedValueResult.Match<Result>(e => {
                Result<KeyFileMap> decryptRes = DecryptKeyFileMap(e);
                if (decryptRes.IsFaulted) {
                    return decryptRes.PullError();
                }

                decryptRes.Tap(e => {
                    // TODO:
                    // 1. Decrypt all files using existing key
                    // 2. Encrypt all files using new key
                    // 3. Save new key
                });

                return Result.Ok();
            }, e => {
                KeyFileMap keyFileMap = new KeyFileMap {
                    Key = key
                };

                Result<string> encryptedKeyFileMapRes = EncryptKeyFileMap(keyFileMap);

                Result res = encryptedKeyFileMapRes.Match<Result>(e => {
                    ekfc.AddKeyFileMap(keyId, e);
                    return Result.Ok();
                }, e => e);

                return res;
            });

            return res;
        }
        catch (Exception ex) {
            return ex;
        }
    }

    public async Task<Result<IKey>> GetKeyAsync(string keyId) {
        try {
            Result<EncryptedKeyFileContent> ekfc = await GetEncryptedKeyFileContentAsync();
            if (ekfc.IsFaulted) {
                return ekfc.PullError<IKey>();
            }

            Result<string> encryptedValueResult = GetEncryptedValueForKeyIdAsync(keyId, ekfc.Value!);
            if (encryptedValueResult.IsFaulted) {
                return encryptedValueResult.PullError<IKey>();
            }

            Result<KeyFileMap> decryptRes = DecryptKeyFileMap(encryptedValueResult.Value);
            return decryptRes.Map(e => e.Key);
        }
        catch (Exception ex) {
            return ex;
        }
    }

    private static Result<string> GetEncryptedValueForKeyIdAsync(string keyId, EncryptedKeyFileContent ekfc) {
        string? encryptedValue = ekfc.GetEncryptedValue(keyId);
        if (encryptedValue is null) {
            return new KeyNotFoundException(keyId);
        }

        return encryptedValue;
    }

    private Result<string> EncryptKeyFileMap(KeyFileMap keyFileMap) {
        try {
            string keyFileMapJson = JsonSerializer.Serialize(keyFileMap);
            byte[] rawContent  = GlobalEnvironment.Encoding.GetBytes(keyFileMapJson);
            AesCryptographer aesCryptographer = new AesCryptographer();
            using AesKey key = masterKey.ConvertToAesKey();
            byte[] encryptedContent = aesCryptographer.Encrypt(rawContent, key);
            return Convert.ToBase64String(encryptedContent);
        }
        catch (Exception ex) {
            return ex;
        }
    }

    private Result<KeyFileMap> DecryptKeyFileMap(string? encryptedKeyFileMap) {
        try {
            if (encryptedKeyFileMap is null) {
                return Result<KeyFileMap>.Fail(new ArgumentNullException(nameof(encryptedKeyFileMap)));
            }

            byte[] buffer = Convert.FromBase64String(encryptedKeyFileMap);
            AesCryptographer aesCryptographer = new AesCryptographer();
            using AesKey key = masterKey.ConvertToAesKey();
            byte[] rawContent = aesCryptographer.Decrypt(buffer, key);
            string sContent = GlobalEnvironment.Encoding.GetString(rawContent);

            KeyFileMap? content = JsonSerializer.Deserialize<KeyFileMap>(sContent);
            if (content is null) {
                return Result<KeyFileMap>.Fail(new NullReferenceException(nameof(content)));
            }

            return Result<KeyFileMap>.Ok(content);
        }
        catch (Exception ex) {
            return ex;
        }
    }

    public async Task<Result<EncryptedKeyFileContent>> GetEncryptedKeyFileContentAsync() {
        try {
            byte[] buffer;
            using (FileStream fs = File.OpenRead(KeyFile)) {
                buffer = await fs.ReadAsync();
            }

            AesCryptographer aesCryptographer = new AesCryptographer();
            byte[] rawContent = aesCryptographer.Decrypt(buffer, masterKey.ConvertToAesKey());

            EncryptedKeyFileContent? content = JsonSerializer.Deserialize<EncryptedKeyFileContent>(rawContent);
            if (content is null) {
                return new NullReferenceException(nameof(content));
            }

            return content;
        }
        catch (Exception ex) {
            return ex;
        }
    }
}

public sealed class EncryptedKeyFileContent {
    private readonly Dictionary<string, string> encryptedKeyFileMap;

    [JsonConstructor]
    internal EncryptedKeyFileContent(Dictionary<string, string> encryptedKeyFileMap) {
        this.encryptedKeyFileMap = encryptedKeyFileMap;
    }

    public string? GetEncryptedValue(string keyId) {
        if (TryGetEncryptedValue(keyId, out string? encryptedValue)) {
            return encryptedValue;
        }

        return null;
    }

    public bool TryGetEncryptedValue(string keyId, [NotNullWhen(true)] out string? encryptedValue) {
        return encryptedKeyFileMap.TryGetValue(keyId, out encryptedValue);
    }

    public void AddKeyFileMap(string keyId, string encryptedKeyFileMap) {
        this.encryptedKeyFileMap.Add(keyId, encryptedKeyFileMap);
    }
}

public sealed class KeyFileMap : IEquatable<KeyFileMap> {
    public required IKey Key { get; init; }
    public List<string> Files { get; init; } = [];

    [JsonConstructor]
    public KeyFileMap() {

    }

    public bool Equals(KeyFileMap? other) {
        return Key == other?.Key && Files.SequenceEqual(other.Files);
    }

    public override bool Equals(object? obj) {
        return Equals(obj as KeyFileMap);
    }

    public override int GetHashCode() {
        int filesHashcode = HBHashCode.CombineSequence(Files);
        return HBHashCode.Combine(Key, filesHashcode);
    }
}