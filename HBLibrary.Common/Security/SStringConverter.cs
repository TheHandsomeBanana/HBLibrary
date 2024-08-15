using System.Runtime.InteropServices;
using System.Security;

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

    public static SecureString StringToSecureString(string input) {
        if (string.IsNullOrEmpty(input)) {
            throw new ArgumentNullException(nameof(input));
        }

        SecureString secureString = new SecureString();
        foreach (char c in input) {
            secureString.AppendChar(c);
        }
        secureString.MakeReadOnly();
        return secureString;
    }
}
