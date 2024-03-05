using HBLibrary.Common.Process;
using HBLibrary.Services.IO.Archiving.WinRAR.Options;
using HBLibrary.Services.IO.Compression;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HBLibrary.Services.IO.Archiving.WinRAR;
/// <summary>
/// Utilizes the WinRAR command line for RAR compression
/// </summary>
public interface IWinRARCompressor : ICompressor {
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
    /// Timeout in milliseconds for synchronous <see cref="Compress(string, string, WinRARCompressionOptions)"/> / <see cref="Extract(string, string, WinRARExtractionOptions)"/> operations
    /// </summary>
    int? ProcessTimeout { get; set; }

#if NET5_0_OR_GREATER
    Task CompressAsync(Archive archive, WinRARCompressionOptions settings, CancellationToken token = default);
#endif

    void Compress(Archive archive, WinRARCompressionOptions settings);
}

