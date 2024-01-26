using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HBLibrary.Services.IO.Compression.WinRAR {
    public interface IWinRARCompressionService : ICompressionService {
        void Compress(string source, string destinationArchive, WinRARCompressionSettings settings);
        void Extract(string sourceArchive, string destinationDirectory, WinRARExtractionSettings settings);

#if NET5_0_OR_GREATER
        Task? CompressAsync(string source, string destinationArchive, WinRARCompressionSettings settings, CancellationToken token = default);
        Task? ExtractAsync(string sourceArchive, string destinationDirectory, WinRARExtractionSettings settings, CancellationToken token = default);
#endif
    }
}
