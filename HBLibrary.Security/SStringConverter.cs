using System.Runtime.InteropServices;
using System.Security;

namespace HBLibrary.Security;
public static class SStringConverter {
    public static string? SecureStringToString(this SecureString value, bool dispose = false) {
        nint valuePtr = IntPtr.Zero;
        try {
            valuePtr = Marshal.SecureStringToGlobalAllocUnicode(value);
            return Marshal.PtrToStringUni(valuePtr);
        }
        finally {
            if (dispose) {
                Marshal.ZeroFreeGlobalAllocUnicode(valuePtr);
                value.Dispose();
            }
        }
    }

    public static SecureString StringToSecureString(this string input) {
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
