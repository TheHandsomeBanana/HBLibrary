using System.Text.Json.Serialization;
using System.Xml;
using System.Xml.Serialization;

namespace HBLibrary.Interface.Security.Keys;

[Serializable]
public class RsaKey : IKey
{
    [XmlIgnore]
    [JsonIgnore]
    public byte[] Key { get; set; }
    public int KeySize { get; set; }
    public bool IsPublic { get; set; }

    [XmlElement("Key")]
    [JsonPropertyName("Key")]
    public string Base64Key
    {
        get => Convert.ToBase64String(Key);
        set => Key = string.IsNullOrEmpty(value) ? [] : Convert.FromBase64String(value);
    }

    [JsonIgnore]
    [XmlIgnore]
    public string Name => nameof(RsaKey);

    public RsaKey(byte[] key, int keySize, bool isPublic)
    {
        Key = key;
        KeySize = keySize;
        IsPublic = isPublic;
    }
}
