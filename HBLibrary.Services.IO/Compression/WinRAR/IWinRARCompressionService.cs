using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HBLibrary.Services.IO.Compression.WinRAR {
    public interface IWinRARCompressionService : ICompressionService {
        void Compress(string sourceFile, string destinationArchive, WinRARCompressionSettings settings);
        void Extract(string sourceArchive, string destinationDirectory, WinRARExtractionSettings settings);

#if NET8_0_OR_GREATER
        Task? CompressAsync(string sourceFile, string destinationArchive, WinRARCompressionSettings settings);
        Task? ExtractAsync(string sourceArchive, string destinationDirectory, WinRARExtractionSettings settings);
#endif
    }
}
