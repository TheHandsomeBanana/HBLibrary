using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace HBLibrary.Services.Security.Cryptography.Keys;
[Serializable]
public class DPEntropy : IKey {
    [JsonIgnore]
    [XmlIgnore]
    public string Name => nameof(DPEntropy);
    [JsonIgnore]
    [XmlIgnore]
    public byte[] Key { get; set; } = [];

    [XmlElement("Key")]
    [JsonPropertyName("Key")]
    public string Base64Key {
        get => Convert.ToBase64String(Key);
        set => Key = string.IsNullOrEmpty(value) ? [] : Convert.FromBase64String(value);
    }

    public DPEntropy(byte[] key) {
        Key = key;
    }
}
