using HBLibrary.Common.Process;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HBLibrary.Services.IO.Archiving.WinRAR;
public interface IWinRARExtractor : IExtractor {
    event EventHandler<ProcessExitEventArgs>? OnProcessExit;
    event EventHandler<ProcessStdStreamEventArgs>? OnOutputDataReceived;
    event EventHandler<ProcessStdStreamEventArgs>? OnErrorDataReceived;
    /// <summary>
    /// Get the last operation's standard output
    /// </summary>
    string StdOutput { get; }
    /// <summary>
    /// Get the last operation's standard error
    /// </summary>
    string StdError { get; }
    /// <summary>
    /// Timeout in milliseconds for synchronous <see cref="Compress(string, string, WinRARCompressionSettings)"/> / <see cref="Extract(string, string, WinRARExtractionSettings)"/> operations
    /// </summary>
    int? ProcessTimeout { get; set; }

#if NET5_0_OR_GREATER
    Task ExtractAsync(FileSnapshot sourceArchive, DirectorySnapshot destinationDirectory, WinRARExtractionSettings settings, CancellationToken token = default);
#endif
    void Extract(FileSnapshot sourceArchive, DirectorySnapshot destinationDirectory, WinRARExtractionSettings settings);
}
