using HBLibrary.Common.Process;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HBLibrary.Services.IO.Compression.WinRAR {
    /// <summary>
    /// Utilizes the WinRAR command line for RAR compression
    /// </summary>
    public interface IWinRARCompressionService : ICompressionService {
        event EventHandler<ProcessExitEventArgs>? OnProcessExit;
        event DataReceivedEventHandler? OnErrorDataReceived;
        event DataReceivedEventHandler? OnOutputDataReceived;
        /// <summary>
        /// Timeout in milliseconds for synchronous <see cref="Compress(string, string, WinRARCompressionSettings)"/> / <see cref="Extract(string, string, WinRARExtractionSettings)"/> operations
        /// </summary>
        int? ProcessTimeout { get; set; }

        void Compress(string source, string destinationArchive, WinRARCompressionSettings settings);
        void Extract(string sourceArchive, string destinationDirectory, WinRARExtractionSettings settings);

#if NET5_0_OR_GREATER
        Task CompressAsync(string source, string destinationArchive, WinRARCompressionSettings settings, CancellationToken token = default);
        Task ExtractAsync(string sourceArchive, string destinationDirectory, WinRARExtractionSettings settings, CancellationToken token = default);
#endif
    }
}
