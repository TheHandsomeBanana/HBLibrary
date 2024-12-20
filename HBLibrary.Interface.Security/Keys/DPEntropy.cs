using System.Text.Json.Serialization;
using System.Xml.Serialization;

namespace HBLibrary.Interface.Security.Keys;
[Serializable]
public class DPEntropy : IKey
{
    private byte[]? key;

    [JsonIgnore]
    [XmlIgnore]
    public bool IsDisposed { get; private set; }

    [JsonIgnore]
    [XmlIgnore]
    public string Name => nameof(DPEntropy);

    [JsonIgnore]
    [XmlIgnore]
    public byte[]? Key {
        get {
            if (IsDisposed) {
                throw new ObjectDisposedException(nameof(RsaKey));
            }

            return key;
        }
        set {
            if (IsDisposed) {
                throw new ObjectDisposedException(nameof(RsaKey));
            }

            key = value;
        }
    }

    [XmlElement("Key")]
    [JsonPropertyName("Key")]
    public string Base64Key
    {
        get => Convert.ToBase64String(Key!);
        set => Key = string.IsNullOrEmpty(value) ? [] : Convert.FromBase64String(value);
    }


    public DPEntropy(byte[] key)
    {
        Key = key;
    }

    public void Dispose() {
        if (IsDisposed) {
            throw new ObjectDisposedException(nameof(RsaKey));
        }

        if (Key is not null) {
            Array.Clear(Key, 0, Key.Length);
            Key = null;
        }
    }
}
