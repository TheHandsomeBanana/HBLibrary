using System.Text.Json.Serialization;
using System.Xml.Serialization;

namespace HBLibrary.Interface.Security.Keys;
[Serializable]
public class DPEntropy : IKey
{
    [JsonIgnore]
    [XmlIgnore]
    public string Name => nameof(DPEntropy);
    [JsonIgnore]
    [XmlIgnore]
    public byte[] Key { get; set; } = [];

    [XmlElement("Key")]
    [JsonPropertyName("Key")]
    public string Base64Key
    {
        get => Convert.ToBase64String(Key);
        set => Key = string.IsNullOrEmpty(value) ? [] : Convert.FromBase64String(value);
    }

    public DPEntropy(byte[] key)
    {
        Key = key;
    }
}
