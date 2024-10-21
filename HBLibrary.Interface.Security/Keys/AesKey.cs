using System.Text.Json.Serialization;
using System.Xml.Serialization;

namespace HBLibrary.Interface.Security.Keys;
[Serializable]
public class AesKey : IKey
{
    [JsonIgnore]
    [XmlIgnore]
    public byte[] Key { get; set; }
    [JsonIgnore]
    [XmlIgnore]
    public byte[] IV { get; set; }

    [XmlElement("Key")]
    [JsonPropertyName("Key")]
    public string Base64Key
    {
        get => Convert.ToBase64String(Key);
        set => Key = string.IsNullOrEmpty(value) ? [] : Convert.FromBase64String(value);
    }

    [XmlElement("IV")]
    [JsonPropertyName("IV")]
    public string Base64IV
    {
        get => Convert.ToBase64String(IV);
        set => IV = string.IsNullOrEmpty(value) ? [] : Convert.FromBase64String(value);
    }

    [JsonIgnore]
    [XmlIgnore]
    public string Name => nameof(AesKey);

    public AesKey(byte[] key, byte[] iv)
    {
        Key = key;
        IV = iv;
    }

    public AesKey()
    {
        Key = [];
        IV = [];
    }
}
