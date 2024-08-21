using System.Security.Cryptography;

namespace HBLibrary.Common.Extensions;
public static class GuidConverter {

    public static Guid ToGuid(this string value) {
        using SHA256 sha256 = SHA256.Create();
        byte[] hash = sha256.ComputeHash(GlobalEnvironment.Encoding.GetBytes(value));
        Span<byte> truncatedHash = stackalloc byte[16];
        hash.AsSpan(0, 16).CopyTo(truncatedHash);
        return new Guid(truncatedHash.ToArray());
    }

    public static string ToGuidString(this string value) {
        return value.ToGuid().ToString();
    }
}
