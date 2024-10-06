using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HBLibrary.Common.Extensions;

namespace HBLibrary.Common;

public static class UnifiedFile {
    public static Task<byte[]> ReadAllBytesAsync(string fullPath) {
#if NET5_0_OR_GREATER
        return File.ReadAllBytesAsync(fullPath);
#elif NET472_OR_GREATER
        using(FileStream fs = new FileStream(fullPath, FileMode.Open, FileAccess.Read)) {
            return fs.ReadAsync();    
        }
#endif
    }

    public static Task WriteAllBytesAsync(string fullPath, byte[] content) {
#if NET5_0_OR_GREATER
        return File.WriteAllBytesAsync(fullPath, content);
#elif NET472_OR_GREATER
        using(FileStream fs = new FileStream(fullPath, FileMode.Open, FileAccess.Read)) {
            return fs.WriteAsync(content);    
        }
#endif
    }

    public static Task<string> ReadAllTextAsync(string fullPath) {
#if NET5_0_OR_GREATER
        return File.ReadAllTextAsync(fullPath);
#elif NET472_OR_GREATER
        using(StreamReader sw = new StreamReader(fullPath)) {
            return sw.ReadToEndAsync();    
        }
#endif
    }

    public static Task WriteAllTextAsync(string fullPath, string content) {
#if NET5_0_OR_GREATER
        return File.WriteAllTextAsync(fullPath, content);
#elif NET472_OR_GREATER
        using(StreamWriter sw = new StreamWriter(fullPath)) {
            return sw.WriteAsync(content);    
        }
#endif
    }
}
