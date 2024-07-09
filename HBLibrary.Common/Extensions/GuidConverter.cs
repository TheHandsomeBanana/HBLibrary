using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace HBLibrary.Common.Extensions;
public static class GuidConverter {

    public static Guid ToGuid(this string value) {
        using MD5 md5 = MD5.Create();
        byte[] hash = md5.ComputeHash(GlobalEnvironment.Encoding.GetBytes(value));
        return new Guid(hash);
    }

    public static string ToGuidString(this string value) {
        return value.ToGuid().ToString();
    }
}
