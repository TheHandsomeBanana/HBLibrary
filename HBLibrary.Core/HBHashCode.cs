using System.Security.Cryptography;
using System.Text;
using System.Text.Json;

namespace HBLibrary.Core;
public readonly struct HBHashCode {
    public static int Combine<T1, T2>(T1 item1, T2 item2) {
#if NET5_0_OR_GREATER
        return HashCode.Combine(item1, item2);
#elif NET472_OR_GREATER
        int hash1 = item1?.GetHashCode() ?? 0;
        int hash2 = item2?.GetHashCode() ?? 0;

        return Combine(hash1, hash2);
#endif
    }

    public static int Combine<T1, T2, T3>(T1 item1, T2 item2, T3 item3) {
#if NET5_0_OR_GREATER
        return HashCode.Combine(item1, item2, item3);
#elif NET472_OR_GREATER
        int hash1 = item1?.GetHashCode() ?? 0;
        int hash2 = item2?.GetHashCode() ?? 0;
        int hash3 = item3?.GetHashCode() ?? 0;

        return Combine(hash1, hash2, hash3);
#endif
    }

    public static int Combine<T1, T2, T3, T4>(T1 item1, T2 item2, T3 item3, T4 item4) {
#if NET5_0_OR_GREATER
        return HashCode.Combine(item1, item2, item3, item4);
#elif NET472_OR_GREATER
        int hash1 = item1?.GetHashCode() ?? 0;
        int hash2 = item2?.GetHashCode() ?? 0;
        int hash3 = item3?.GetHashCode() ?? 0;
        int hash4 = item4?.GetHashCode() ?? 0;

        return Combine(hash1, hash2, hash3, hash4);
#endif
    }

    public static int Combine<T1, T2, T3, T4, T5>(T1 item1, T2 item2, T3 item3, T4 item4, T5 item5) {
#if NET5_0_OR_GREATER
        return HashCode.Combine(item1, item2, item3, item4, item5);
#elif NET472_OR_GREATER
        int hash1 = item1?.GetHashCode() ?? 0;
        int hash2 = item2?.GetHashCode() ?? 0;
        int hash3 = item3?.GetHashCode() ?? 0;
        int hash4 = item4?.GetHashCode() ?? 0;
        int hash5 = item5?.GetHashCode() ?? 0;

        return Combine(hash1, hash2, hash3, hash4, hash5);
#endif
    }

    public static int Combine<T1, T2, T3, T4, T5, T6>(T1 item1, T2 item2, T3 item3, T4 item4, T5 item5, T6 item6) {
#if NET5_0_OR_GREATER
        return HashCode.Combine(item1, item2, item3, item4, item5, item6);
#elif NET472_OR_GREATER
        int hash1 = item1?.GetHashCode() ?? 0;
        int hash2 = item2?.GetHashCode() ?? 0;
        int hash3 = item3?.GetHashCode() ?? 0;
        int hash4 = item4?.GetHashCode() ?? 0;
        int hash5 = item5?.GetHashCode() ?? 0;
        int hash6 = item6?.GetHashCode() ?? 0;

        return Combine(hash1, hash2, hash3, hash4, hash5, hash6);
#endif
    }

    public static int Combine<T1, T2, T3, T4, T5, T6, T7>(T1 item1, T2 item2, T3 item3, T4 item4, T5 item5, T6 item6, T7 item7) {
#if NET5_0_OR_GREATER
        return HashCode.Combine(item1, item2, item3, item4, item5, item6, item7);
#elif NET472_OR_GREATER
        int hash1 = item1?.GetHashCode() ?? 0;
        int hash2 = item2?.GetHashCode() ?? 0;
        int hash3 = item3?.GetHashCode() ?? 0;
        int hash4 = item4?.GetHashCode() ?? 0;
        int hash5 = item5?.GetHashCode() ?? 0;
        int hash6 = item6?.GetHashCode() ?? 0;
        int hash7 = item7?.GetHashCode() ?? 0;

        return Combine(hash1, hash2, hash3, hash4, hash5, hash6, hash7);
#endif
    }

    public static int Combine<T1, T2, T3, T4, T5, T6, T7, T8>(T1 item1, T2 item2, T3 item3, T4 item4, T5 item5, T6 item6, T7 item7, T8 item8)
        where T1 : notnull where T2 : notnull where T3 : notnull where T4 : notnull
        where T5 : notnull where T6 : notnull where T7 : notnull where T8 : notnull {
#if NET5_0_OR_GREATER
        return HashCode.Combine(item1, item2, item3, item4, item5, item6, item7, item8);
#elif NET472_OR_GREATER
        int hash1 = item1?.GetHashCode() ?? 0;
        int hash2 = item2?.GetHashCode() ?? 0;
        int hash3 = item3?.GetHashCode() ?? 0;
        int hash4 = item4?.GetHashCode() ?? 0;
        int hash5 = item5?.GetHashCode() ?? 0;
        int hash6 = item6?.GetHashCode() ?? 0;
        int hash7 = item7?.GetHashCode() ?? 0;
        int hash8 = item8?.GetHashCode() ?? 0;

        return Combine(hash1, hash2, hash3, hash4, hash5, hash6, hash7, hash8);
#endif
    }

#if NET472_OR_GREATER

    private static readonly int randomSeed = GenerateSeed();

    private static int Combine(params int[] hashes) {
        int hash = randomSeed;

        // Use prime numbers to minimize collisions
        foreach (int h in hashes) {
            hash = hash * 17 + h;
            hash = hash * 23 + h;
            hash = hash * 31 + h;
            hash += hash << 13;
            hash ^= hash >> 7;
            hash += hash << 3;
            hash ^= hash >> 17;
            hash += hash << 5;
        }

        return hash;

    }

    private static int GenerateSeed() {
        byte[] bytes = new byte[4];
        using (RandomNumberGenerator rng = RandomNumberGenerator.Create()) {
            rng.GetBytes(bytes);
        }

        return BitConverter.ToInt32(bytes, 0);
    }
#endif

    /// <summary>
    /// Combines two items by serializing them to JSON and then hashing the combined JSON string using SHA512.
    /// The items will be serialized to JSON before hashing.
    /// </summary>
    public static string CombineSHA512<T1, T2>(T1 item1, T2 item2) where T1 : notnull where T2 : notnull {
        string combinedJson = JsonSerializer.Serialize(new { item1, item2 });
        return ComputeSHA512(combinedJson);
    }

    /// <summary>
    /// Combines three items by serializing them to JSON and then hashing the combined JSON string using SHA512.
    /// The items will be serialized to JSON before hashing.
    /// </summary>
    public static string CombineSHA512<T1, T2, T3>(T1 item1, T2 item2, T3 item3) where T1 : notnull where T2 : notnull where T3 : notnull {
        string combinedJson = JsonSerializer.Serialize(new { item1, item2, item3 });
        return ComputeSHA512(combinedJson);
    }

    /// <summary>
    /// Combines four items by serializing them to JSON and then hashing the combined JSON string using SHA512.
    /// The items will be serialized to JSON before hashing.
    /// </summary>
    public static string CombineSHA512<T1, T2, T3, T4>(T1 item1, T2 item2, T3 item3, T4 item4) {

        string combinedJson = JsonSerializer.Serialize(new { item1, item2, item3, item4 });
        return ComputeSHA512(combinedJson);
    }

    /// <summary>
    /// Combines five items by serializing them to JSON and then hashing the combined JSON string using SHA512.
    /// The items will be serialized to JSON before hashing.
    /// </summary>
    public static string CombineSHA512<T1, T2, T3, T4, T5>(T1 item1, T2 item2, T3 item3, T4 item4, T5 item5) {

        string combinedJson = JsonSerializer.Serialize(new { item1, item2, item3, item4, item5 });
        return ComputeSHA512(combinedJson);
    }

    /// <summary>
    /// Combines six items by serializing them to JSON and then hashing the combined JSON string using SHA512.
    /// The items will be serialized to JSON before hashing.
    /// </summary>
    public static string CombineSHA512<T1, T2, T3, T4, T5, T6>(T1 item1, T2 item2, T3 item3, T4 item4, T5 item5, T6 item6) {

        string combinedJson = JsonSerializer.Serialize(new { item1, item2, item3, item4, item5, item6 });
        return ComputeSHA512(combinedJson);
    }

    /// <summary>
    /// Combines seven items by serializing them to JSON and then hashing the combined JSON string using SHA512.
    /// The items will be serialized to JSON before hashing.
    /// </summary>
    public static string CombineSHA512<T1, T2, T3, T4, T5, T6, T7>(T1 item1, T2 item2, T3 item3, T4 item4, T5 item5, T6 item6, T7 item7) {

        string combinedJson = JsonSerializer.Serialize(new { item1, item2, item3, item4, item5, item6, item7 });
        return ComputeSHA512(combinedJson);
    }

    /// <summary>
    /// Combines eight items by serializing them to JSON and then hashing the combined JSON string using SHA512.
    /// The items will be serialized to JSON before hashing.
    /// </summary>
    public static string CombineSHA512<T1, T2, T3, T4, T5, T6, T7, T8>(T1 item1, T2 item2, T3 item3, T4 item4, T5 item5, T6 item6, T7 item7, T8 item8) {

        string combinedJson = JsonSerializer.Serialize(new { item1, item2, item3, item4, item5, item6, item7, item8 });
        return ComputeSHA512(combinedJson);
    }

    /// <summary>
    /// Combines two items by serializing them to JSON and then hashing the combined JSON string using SHA265.
    /// The items will be serialized to JSON before hashing.
    /// </summary>
    public static string CombineSHA256<T1, T2>(T1 item1, T2 item2) {
        string combinedJson = JsonSerializer.Serialize(new { item1, item2 });
        return ComputeSHA265(combinedJson);
    }

    /// <summary>
    /// Combines three items by serializing them to JSON and then hashing the combined JSON string using SHA265.
    /// The items will be serialized to JSON before hashing.
    /// </summary>
    public static string CombineSHA256<T1, T2, T3>(T1 item1, T2 item2, T3 item3) {
        string combinedJson = JsonSerializer.Serialize(new { item1, item2, item3 });
        return ComputeSHA265(combinedJson);
    }

    /// <summary>
    /// Combines four items by serializing them to JSON and then hashing the combined JSON string using SHA265.
    /// The items will be serialized to JSON before hashing.
    /// </summary>
    public static string CombineSHA256<T1, T2, T3, T4>(T1 item1, T2 item2, T3 item3, T4 item4) {

        string combinedJson = JsonSerializer.Serialize(new { item1, item2, item3, item4 });
        return ComputeSHA265(combinedJson);
    }

    /// <summary>
    /// Combines five items by serializing them to JSON and then hashing the combined JSON string using SHA265.
    /// The items will be serialized to JSON before hashing.
    /// </summary>
    public static string CombineSHA256<T1, T2, T3, T4, T5>(T1 item1, T2 item2, T3 item3, T4 item4, T5 item5) {
        string combinedJson = JsonSerializer.Serialize(new { item1, item2, item3, item4, item5 });
        return ComputeSHA265(combinedJson);
    }

    /// <summary>
    /// Combines six items by serializing them to JSON and then hashing the combined JSON string using SHA265.
    /// The items will be serialized to JSON before hashing.
    /// </summary>
    public static string CombineSHA256<T1, T2, T3, T4, T5, T6>(T1 item1, T2 item2, T3 item3, T4 item4, T5 item5, T6 item6) {

        string combinedJson = JsonSerializer.Serialize(new { item1, item2, item3, item4, item5, item6 });
        return ComputeSHA265(combinedJson);
    }

    /// <summary>
    /// Combines seven items by serializing them to JSON and then hashing the combined JSON string using SHA265.
    /// The items will be serialized to JSON before hashing.
    /// </summary>
    public static string CombineSHA256<T1, T2, T3, T4, T5, T6, T7>(T1 item1, T2 item2, T3 item3, T4 item4, T5 item5, T6 item6, T7 item7) {

        string combinedJson = JsonSerializer.Serialize(new { item1, item2, item3, item4, item5, item6, item7 });
        return ComputeSHA265(combinedJson);
    }

    /// <summary>
    /// Combines eight items by serializing them to JSON and then hashing the combined JSON string using SHA265.
    /// The items will be serialized to JSON before hashing.
    /// </summary>
    public static string CombineSHA256<T1, T2, T3, T4, T5, T6, T7, T8>(T1 item1, T2 item2, T3 item3, T4 item4, T5 item5, T6 item6, T7 item7, T8 item8) {
        string combinedJson = JsonSerializer.Serialize(new { item1, item2, item3, item4, item5, item6, item7, item8 });
        return ComputeSHA265(combinedJson);
    }

    public static string ComputeSHA265(string data) {
#if NET472_OR_GREATER
        using SHA256 md = SHA256.Create();
        byte[] hash = md.ComputeHash(Encoding.UTF8.GetBytes(data));
#elif NET5_0_OR_GREATER
        byte[] hash = SHA256.HashData(Encoding.UTF8.GetBytes(data));
#endif

        return HexString(hash);
    }

    public static string ComputeSHA512(string data) {
#if NET472_OR_GREATER
        using SHA512 md = SHA512.Create();
        byte[] hash = md.ComputeHash(Encoding.UTF8.GetBytes(data));
#elif NET5_0_OR_GREATER
        byte[] hash = SHA512.HashData(Encoding.UTF8.GetBytes(data));
#endif

        return HexString(hash);
    }

    private static string HexString(byte[] hash) {
        StringBuilder sb = new StringBuilder();
        for (int i = 0; i < hash.Length; i++) {
            sb.Append(hash[i].ToString("x2"));
        }
        return sb.ToString();
    }
}
