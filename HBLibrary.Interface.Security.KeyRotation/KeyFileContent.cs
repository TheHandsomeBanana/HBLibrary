using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace HBLibrary.Interface.Security.KeyRotation;
public class KeyFileContent {
    [JsonPropertyName("keyFileMap")]
    private readonly Dictionary<string, KeyFileMap> keyFileMap;

    [JsonConstructor]
    public KeyFileContent(Dictionary<string, KeyFileMap> keyFileMap) {
        this.keyFileMap = keyFileMap;
    }

    public KeyFileMap? GetKeyFileMap(string keyId) {
        if (TryGetKeyFileMap(keyId, out KeyFileMap? kfm)) {
            return kfm;
        }

        return null;
    }

    public bool TryGetKeyFileMap(string keyId, [NotNullWhen(true)] out KeyFileMap? kfm) {
        return keyFileMap.TryGetValue(keyId, out kfm);
    }

    public void AddOrUpdateKeyFileMap(string keyId, KeyFileMap kfm) {
        this.keyFileMap[keyId] = kfm;
    }
}
