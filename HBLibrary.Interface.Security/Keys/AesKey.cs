using System.Text.Json.Serialization;
using System.Xml.Serialization;

namespace HBLibrary.Interface.Security.Keys;
[Serializable]
public class AesKey : IKey {
    private byte[]? key;
    private byte[]? iv;

    [JsonIgnore]
    [XmlIgnore]
    public bool IsDisposed { get; private set; }

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

    [JsonIgnore]
    [XmlIgnore]
    public byte[]? IV {
        get {
            if (IsDisposed) {
                throw new ObjectDisposedException(nameof(RsaKey));
            }

            return iv;
        }
        set {
            if (IsDisposed) {
                throw new ObjectDisposedException(nameof(RsaKey));
            }

            iv = value;
        }
    }

    [XmlElement("Key")]
    [JsonPropertyName("Key")]
    public string Base64Key {
        get => Convert.ToBase64String(Key!);
        set => Key = string.IsNullOrEmpty(value) ? [] : Convert.FromBase64String(value);
    }

    [XmlElement("IV")]
    [JsonPropertyName("IV")]
    public string Base64IV {
        get => Convert.ToBase64String(IV!);
        set => IV = string.IsNullOrEmpty(value) ? [] : Convert.FromBase64String(value);
    }

    [JsonIgnore]
    [XmlIgnore]
    public string Name => nameof(AesKey);

    public AesKey(byte[] key, byte[] iv) {
        Key = key;
        IV = iv;
    }

    public AesKey() {
        Key = [];
        IV = [];
    }

    public void Dispose() {
        if (IsDisposed) {
            throw new ObjectDisposedException(nameof(RsaKey));
        }

        if (Key is not null) {
            Array.Clear(Key, 0, Key.Length);
            Key = null;
        }

        if (IV is not null) {
            Array.Clear(IV, 0, IV.Length);
            IV = null;
        }

        IsDisposed = true;
    }
}
