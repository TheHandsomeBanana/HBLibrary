using HBLibrary.Interface.Security.KeyRotation;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace HBLibrary.Security.KeyRotation;
public sealed class EncryptedKeyFileContent {
    private readonly Dictionary<string, string> encryptedKeyFileMap;

    [JsonConstructor]
    internal EncryptedKeyFileContent(Dictionary<string, string> encryptedKeyFileMap) {
        this.encryptedKeyFileMap = encryptedKeyFileMap;
    }

    public Dictionary<string, string> GetEncryptedKeyFileMap() {
        return encryptedKeyFileMap;
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

    public void AddOrUpdateKeyFileMap(string keyId, string encryptedKeyFileMap) {
        this.encryptedKeyFileMap[keyId] = encryptedKeyFileMap;
    }
}