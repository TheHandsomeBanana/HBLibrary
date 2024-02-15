using HBLibrary.Common.Process;
using HBLibrary.Services.IO.Archiving.WinRAR.ConfigModels;
using HBLibrary.Services.IO.Compression;
using HBLibrary.Services.IO.Compression.WinRAR;
using HBLibrary.Services.IO.Exceptions;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HBLibrary.Services.IO.Archiving.WinRAR;
public class WinRARCompressor : WinRARArchiverBase, IWinRARCompressor {

    public void Compress(string source, string destinationArchive, WinRARCompressionSettings settings) {
        CompressInternal(destinationArchive, settings, [source]);
    }

    public void Extract(string sourceArchive, string destinationDirectory, WinRARExtractionSettings settings) {
        StartReading();

        string arguments = settings.ToString()! + $"\"{sourceArchive}\" \"{destinationDirectory}\"";
        using Process extractionProcess = CreateProcess(arguments, settings.ExecutableMode, false);
        extractionProcess.Start();

        extractionProcess.BeginErrorReadLine();
        extractionProcess.BeginOutputReadLine();

        extractionProcess.WaitForExit(ProcessTimeout ?? Timeout.Infinite);

        if (!OnProcessExitIsNull()) {
            bool isCanceled = ProcessTimeout.HasValue &&
                (extractionProcess.ExitTime - extractionProcess.StartTime).TotalMilliseconds >= ProcessTimeout;

            ProcessExitEventArgs processExitArgs = CreateProcessExitEventArgs(extractionProcess, isCanceled);

            if (processExitArgs.ExitCode != 0)
                RaiseOnErrorDataReceived(new ProcessStdStreamEventArgs(processExitArgs.GetDescription()));


            RaiseOnProcessExit(CreateProcessExitEventArgs(extractionProcess, isCanceled));
        }

        StopReading();
    }

#if NET5_0_OR_GREATER
    public Task CompressAsync(string source, string destinationArchive, WinRARCompressionSettings settings, CancellationToken token = default) {
        return CompressAsyncInternal(destinationArchive, settings, [source], token);
    }

    public async Task ExtractAsync(string sourceArchive, string destinationDirectory, WinRARExtractionSettings settings, CancellationToken token = default) {
        StartReading();

        string arguments = settings.ToString()! + $"\"{sourceArchive}\" \"{destinationDirectory}\"";
        using Process extractionProcess = CreateProcess(arguments, settings.ExecutableMode, false);
        extractionProcess.Start();

        extractionProcess.BeginErrorReadLine();
        extractionProcess.BeginOutputReadLine();

        await extractionProcess.WaitForExitAsync(token);

        RaiseOnProcessExit(CreateProcessExitEventArgs(extractionProcess, token.IsCancellationRequested));
        StopReading();
    }
#endif

    public void Compress(Archive archive, WinRARCompressionSettings settings) {
        CompressInternal(archive.Name, settings, archive.GetEntries());
    }

#if NET5_0_OR_GREATER
    public Task CompressAsync(Archive archive, WinRARCompressionSettings settings, CancellationToken token = default) {
        return CompressAsyncInternal(archive.Name, settings, archive.GetEntries(), token);
    }
#endif

    public void Compress(Archive archive) {
        Compress(archive, WinRARCompressionSettings.Default);
    }

    #region Helper
    private void CompressInternal(string destinationArchive, WinRARCompressionSettings settings, IEnumerable<string> sources) {
        StartReading();

        destinationArchive = settings.SetExtension(destinationArchive);

        string arguments = settings.ToString()! + $"\"{destinationArchive}\" {string.Join(" ", sources.Select(e => $"\"{e}\""))}";
        using Process compressionProcess = CreateProcess(arguments, settings.ExecutableMode, true);
        compressionProcess.Start();

        compressionProcess.BeginErrorReadLine();
        compressionProcess.BeginOutputReadLine();

        compressionProcess.WaitForExit(ProcessTimeout ?? Timeout.Infinite);

        if (!OnProcessExitIsNull()) {
            bool isCanceled = ProcessTimeout.HasValue &&
                (compressionProcess.ExitTime - compressionProcess.StartTime).TotalMilliseconds >= ProcessTimeout;

            ProcessExitEventArgs processExitArgs = CreateProcessExitEventArgs(compressionProcess, isCanceled);

            if (processExitArgs.ExitCode != 0)
                RaiseOnErrorDataReceived(new ProcessStdStreamEventArgs(processExitArgs.GetDescription()));

            RaiseOnProcessExit(processExitArgs);
        }

        StopReading();
    }

#if NET5_0_OR_GREATER
    private async Task CompressAsyncInternal(string destinationArchive, WinRARCompressionSettings settings, IEnumerable<string> sources, CancellationToken token = default) {
        StartReading();

        destinationArchive = settings.SetExtension(destinationArchive);

        string arguments = settings.ToString()! + $"\"{destinationArchive}\" {string.Join(" ", sources.Select(e => $"\"{e}\""))}";
        using Process compressionProcess = CreateProcess(arguments, settings.ExecutableMode, true);
        compressionProcess.Start();

        compressionProcess.BeginErrorReadLine();
        compressionProcess.BeginOutputReadLine();

        await compressionProcess.WaitForExitAsync(token);
        RaiseOnProcessExit(CreateProcessExitEventArgs(compressionProcess, token.IsCancellationRequested));
        StopReading();
    }
#endif

    private Process CreateProcess(string arguments, WinRARExecutableMode executableMode, bool compress) {
        ProcessStartInfo startInfo = new ProcessStartInfo {
            FileName = WinRARPath + (executableMode == WinRARExecutableMode.WinRAR
            ? "\\WinRAR.exe"
            : compress ? "\\Rar.exe" : "\\UnRAR.exe"),

            Arguments = arguments,
            RedirectStandardError = true,
            RedirectStandardOutput = true,
            CreateNoWindow = true,
            UseShellExecute = false,
        };

        Process process = new Process {
            EnableRaisingEvents = true,
            StartInfo = startInfo
        };

        process.OutputDataReceived += OutputDataReceived;

        return process;
    }

    private void OutputDataReceived(object sender, DataReceivedEventArgs e) {
        if (string.IsNullOrEmpty(e.Data))
            return;

        // WinRAR writes errors to stdout for some crazy idk reason
        if (e.Data.StartsWith("error", StringComparison.CurrentCultureIgnoreCase)) {
            StandardError!.AppendLine(e.Data);
            RaiseOnErrorDataReceived(sender, new ProcessStdStreamEventArgs(e.Data));
        }
        else {
            StandardOutput!.AppendLine(e.Data);
            RaiseOnOutputDataReceived(sender, new ProcessStdStreamEventArgs(e.Data));
        }
    }

    
    #endregion
}
