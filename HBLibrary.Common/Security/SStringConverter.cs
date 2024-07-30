using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;
using System.Threading.Tasks;

namespace HBLibrary.Common.Security;
public static class SStringConverter {
    public static string? SecureStringToString(SecureString value) {
        IntPtr valuePtr = IntPtr.Zero;
        try {
            valuePtr = Marshal.SecureStringToGlobalAllocUnicode(value);
            return Marshal.PtrToStringUni(valuePtr);
        }
        finally {
            Marshal.ZeroFreeGlobalAllocUnicode(valuePtr);
        }
    }
}
