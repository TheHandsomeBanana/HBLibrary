using System.Security.Cryptography;

namespace HBLibrary.Services.Security.Cryptography.Keys;
public class RsaKey : IKey {
    public byte[] Key { get; set; }
    public int KeySize { get; set; }
    public bool IsPublic { get; set; }

    public string Name => nameof(RsaKey);

    public RsaKey(byte[] key, int keySize, bool isPublic) {
        Key = key;
        KeySize = keySize;
        IsPublic = isPublic;
    }
}
