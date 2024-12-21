using System.Text.Json.Serialization;
using System.Xml;
using System.Xml.Serialization;

namespace HBLibrary.Interface.Security.Keys;

[Serializable]
public class RsaKey : IKey {
    private byte[]? key;
    private int? keySize;
    private bool? isPublic;

    [XmlIgnore]
    [JsonIgnore]
    public bool IsDisposed { get; private set; }

    [XmlIgnore]
    [JsonIgnore]
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

    public int? KeySize {
        get {
            if (IsDisposed) {
                throw new ObjectDisposedException(nameof(RsaKey));
            }

            return keySize;
        }
        set {
            if (IsDisposed) {
                throw new ObjectDisposedException(nameof(RsaKey));
            }

            keySize = value;
        }
    }

    public bool? IsPublic {
        get {
            if (IsDisposed) {
                throw new ObjectDisposedException(nameof(RsaKey));
            }

            return isPublic;
        }
        set {
            if (IsDisposed) {
                throw new ObjectDisposedException(nameof(RsaKey));
            }

            isPublic = value;
        }
    }

    [XmlElement("Key")]
    [JsonPropertyName("Key")]
    public string? Base64Key {
        get => Convert.ToBase64String(Key!);
        set => Key = string.IsNullOrEmpty(value) ? [] : Convert.FromBase64String(value);
    }

    [JsonIgnore]
    [XmlIgnore]
    public string Name => nameof(RsaKey);

    public RsaKey(byte[] key, int keySize, bool isPublic) {
        Key = key;
        KeySize = keySize;
        IsPublic = isPublic;
    }

    public void Dispose() {
        if (IsDisposed) {
            throw new ObjectDisposedException(nameof(RsaKey));
        }

        if(Key is null) {
            return;
        }

        Array.Clear(Key, 0, Key.Length);
        Key = null;
        keySize = null;
        isPublic = null;

        IsDisposed = true;
    }
}
