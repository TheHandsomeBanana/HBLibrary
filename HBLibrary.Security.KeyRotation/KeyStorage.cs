using HBLibrary.Core;
using HBLibrary.Core.Extensions;
using HBLibrary.Core.Limiter;
using HBLibrary.DataStructures;
using HBLibrary.Interface.Security.KeyRotation;
using HBLibrary.Interface.Security.Keys;
using HBLibrary.Security.Aes;
using HBLibrary.Security.Rsa;
using Microsoft.Graph.Models;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
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

    public async Task<Result> AddFilesAsync(string keyId, string[] newFiles) {

    }

    public async Task<Result> SaveKeyAsync(string keyId, IKey key) {
        try {
            Result<EncryptedKeyFileContent> ekfcRes = await GetEncryptedKeyFileContentAsync();
            if (ekfcRes.IsFaulted) {
                return ekfcRes.PullError();
            }

            EncryptedKeyFileContent ekfc = ekfcRes.Value!;

            Result<string> encryptedValueResult = GetEncryptedValueForKeyIdAsync(keyId, ekfc);

            Result res = await encryptedValueResult.MatchAsync<Result>(
                e => HandleExistingEncryptedValueAsync(e, key),
                e => HandleNewKeyIdAsync(keyId, ekfc, key)
            );

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

    private async Task<Result> HandleExistingEncryptedValueAsync(string encryptedValueResult, IKey key) {
        Result<KeyFileMap> decryptRes = DecryptKeyFileMap(encryptedValueResult);

        Result res = await decryptRes.MatchAsync<Result>(async e => {
            // KeyFileMap available -> Reencrypt all present files using the new key
            List<Task<Result>> reencryptTasks = [];
            foreach (string file in e.Files) {
                await IOAsyncLimiter.FileSemaphore.WaitAsync();
                reencryptTasks.Add(Task.Run(async () => {
                    try {
                        return await ReencryptFileAsync(file, e.Key, key);
                    }
                    finally {
                        IOAsyncLimiter.FileSemaphore.Release();
                    }
                }));
            }

            Result[] results = await Task.WhenAll(reencryptTasks);

            ResultCollection resultCollection = ResultCollection.Create(results);
            return resultCollection.AggregateResults();

        }, e => Task.FromResult<Result>(e));

        return Result.Ok();
    }

    private Task<Result> HandleNewKeyIdAsync(string keyId, EncryptedKeyFileContent ekfc, IKey key) {
        KeyFileMap keyFileMap = new KeyFileMap {
            Key = key
        };

        Result<string> encryptedKeyFileMapRes = EncryptKeyFileMap(keyFileMap);
        Result res = encryptedKeyFileMapRes.Match<Result>(e => {
            ekfc.AddKeyFileMap(keyId, e);
            return Result.Ok();
        }, e => e);

        return Task.FromResult(res);
    }

    private static async Task<Result> ReencryptFileAsync(string file, IKey oldKey, IKey newKey) {
        try {
            byte[] encryptedBuffer;
            using (FileStream fs = new FileStream(file, FileMode.Open, FileAccess.Read, FileShare.None, 4096, true)) {
                encryptedBuffer = await fs.ReadAsync();
            }

            byte[] decryptedContent;
            switch (oldKey) {
                case AesKey aesKey:
                    AesCryptographer aesCryptographer = new AesCryptographer();
                    decryptedContent = aesCryptographer.Decrypt(encryptedBuffer, aesKey);
                    break;
                case RsaKeyPair rsaKeyPair:
                    RsaCryptographer rsaCryptographer = new RsaCryptographer();
                    decryptedContent = rsaCryptographer.Decrypt(encryptedBuffer, rsaKeyPair.PrivateKey!);
                    break;
                default:
                    return new NotSupportedException($"{oldKey.Name} is not supported");
            }

            byte[] newEncryptedContent;
            switch (newKey) {
                case AesKey aesKey:
                    AesCryptographer aesCryptographer = new AesCryptographer();
                    newEncryptedContent = aesCryptographer.Encrypt(decryptedContent, aesKey);
                    break;
                case RsaKeyPair rsaKeyPair:
                    RsaCryptographer rsaCryptographer = new RsaCryptographer();
                    newEncryptedContent = rsaCryptographer.Encrypt(decryptedContent, rsaKeyPair.PublicKey!);
                    break;
                default:
                    return new NotSupportedException($"{newKey.Name} is not supported");
            }


            using (FileStream fs = new FileStream(file, FileMode.Create, FileAccess.Write, FileShare.None, 4096, true)) {
                await fs.WriteAsync(newEncryptedContent);
            }

            return Result.Ok();
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
            byte[] rawContent = GlobalEnvironment.Encoding.GetBytes(keyFileMapJson);
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