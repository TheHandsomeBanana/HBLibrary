using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace HBLibrary.Common;
public struct HBHashCode {


    public static int Combine<T1, T2>(T1 item1, T2 item2) where T1 : notnull where T2 : notnull {
#if NET5_0_OR_GREATER
        return HashCode.Combine(item1, item2);
#elif NET472_OR_GREATER
        int hash1 = item1.GetHashCode();
        int hash2 = item2.GetHashCode();

        return Combine(hash1, hash2);
#endif
    }

    public static int Combine<T1, T2, T3>(T1 item1, T2 item2, T3 item3) where T1 : notnull where T2 : notnull where T3 : notnull {
#if NET5_0_OR_GREATER
        return HashCode.Combine(item1, item2, item3);
#elif NET472_OR_GREATER
        int hash1 = item1.GetHashCode();
        int hash2 = item2.GetHashCode();
        int hash3 = item3.GetHashCode();

        return Combine(hash1, hash2, hash3);
#endif
    }

    public static int Combine<T1, T2, T3, T4>(T1 item1, T2 item2, T3 item3, T4 item4)
        where T1 : notnull where T2 : notnull where T3 : notnull where T4 : notnull {
#if NET5_0_OR_GREATER
        return HashCode.Combine(item1, item2, item3, item4);
#elif NET472_OR_GREATER
        int hash1 = item1.GetHashCode();
        int hash2 = item2.GetHashCode();
        int hash3 = item3.GetHashCode();
        int hash4 = item4.GetHashCode();

        return Combine(hash1, hash2, hash3, hash4);
#endif
    }

    public static int Combine<T1, T2, T3, T4, T5>(T1 item1, T2 item2, T3 item3, T4 item4, T5 item5)
    where T1 : notnull where T2 : notnull where T3 : notnull where T4 : notnull where T5 : notnull {
#if NET5_0_OR_GREATER
        return HashCode.Combine(item1, item2, item3, item4, item5);
#elif NET472_OR_GREATER
        int hash1 = item1.GetHashCode();
        int hash2 = item2.GetHashCode();
        int hash3 = item3.GetHashCode();
        int hash4 = item4.GetHashCode();
        int hash5 = item5.GetHashCode();

        return Combine(hash1, hash2, hash3, hash4, hash5);
#endif
    }

    public static int Combine<T1, T2, T3, T4, T5, T6>(T1 item1, T2 item2, T3 item3, T4 item4, T5 item5, T6 item6)
    where T1 : notnull where T2 : notnull where T3 : notnull where T4 : notnull where T5 : notnull where T6 : notnull {
#if NET5_0_OR_GREATER
        return HashCode.Combine(item1, item2, item3, item4, item5, item6);
#elif NET472_OR_GREATER
        int hash1 = item1.GetHashCode();
        int hash2 = item2.GetHashCode();
        int hash3 = item3.GetHashCode();
        int hash4 = item4.GetHashCode();
        int hash5 = item5.GetHashCode();
        int hash6 = item6.GetHashCode();

        return Combine(hash1, hash2, hash3, hash4, hash5, hash6);
#endif
    }

    public static int Combine<T1, T2, T3, T4, T5, T6, T7>(T1 item1, T2 item2, T3 item3, T4 item4, T5 item5, T6 item6, T7 item7)
    where T1 : notnull where T2 : notnull where T3 : notnull where T4 : notnull where T5 : notnull where T6 : notnull where T7 : notnull {
#if NET5_0_OR_GREATER
        return HashCode.Combine(item1, item2, item3, item4, item5, item6, item7);
#elif NET472_OR_GREATER
        int hash1 = item1.GetHashCode();
        int hash2 = item2.GetHashCode();
        int hash3 = item3.GetHashCode();
        int hash4 = item4.GetHashCode();
        int hash5 = item5.GetHashCode();
        int hash6 = item6.GetHashCode();
        int hash7 = item7.GetHashCode();

        return Combine(hash1, hash2, hash3, hash4, hash5, hash6, hash7);
#endif
    }

    public static int Combine<T1, T2, T3, T4, T5, T6, T7, T8>(T1 item1, T2 item2, T3 item3, T4 item4, T5 item5, T6 item6, T7 item7, T8 item8)
        where T1 : notnull where T2 : notnull where T3 : notnull where T4 : notnull
        where T5 : notnull where T6 : notnull where T7 : notnull where T8 : notnull {
#if NET5_0_OR_GREATER
        return HashCode.Combine(item1, item2, item3, item4, item5, item6, item7, item8);
#elif NET472_OR_GREATER
        int hash1 = item1.GetHashCode();
        int hash2 = item2.GetHashCode();
        int hash3 = item3.GetHashCode();
        int hash4 = item4.GetHashCode();
        int hash5 = item5.GetHashCode();
        int hash6 = item6.GetHashCode();
        int hash7 = item7.GetHashCode();
        int hash8 = item8.GetHashCode();

        return Combine(hash1, hash2, hash3, hash4, hash5, hash6, hash7, hash8);
#endif
    }

#if NET472_OR_GREATER

    private static readonly int randomSeed = GenerateSeed();

    private static int Combine(params int[] hashes) {
        unchecked { // No arithmetic checks required
            int hash = randomSeed;

            // Use prime numbers
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
    }

    private static int GenerateSeed() {
        byte[] bytes = new byte[4];
        using (RandomNumberGenerator rng = RandomNumberGenerator.Create()) {
            rng.GetBytes(bytes);
        }

        return BitConverter.ToInt32(bytes, 0);
    }
#endif
}
