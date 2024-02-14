using HBLibrary.Common.Process;
using HBLibrary.Services.IO.Exceptions;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HBLibrary.Services.IO.Compression.WinRAR;
public class WinRARCompressor : IWinRARCompressor {
    private readonly string winRARPath;
    private StringBuilder? standardOutput;
    private StringBuilder? standardError;

    public event EventHandler<ProcessExitEventArgs>? OnProcessExit;
    public event EventHandler<ProcessStdStreamEventArgs>? OnOutputDataReceived;
    public event EventHandler<ProcessStdStreamEventArgs>? OnErrorDataReceived;

    public int? ProcessTimeout { get; set; }

    public string StdOutput { get; set; } = "";
    public string StdError { get; set; } = "";

    public WinRARCompressor() {
        this.winRARPath = WinRARHelper.GetWinRARInstallationPath()
            ?? throw new DirectoryNotFoundException("Could not detect a WinRAR installation.");
    }

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

        if (OnProcessExit != null) {
            bool isCanceled = ProcessTimeout.HasValue &&
                (extractionProcess.ExitTime - extractionProcess.StartTime).TotalMilliseconds >= ProcessTimeout;

            ProcessExitEventArgs processExitArgs = CreateProcessExitEventArgs(extractionProcess, isCanceled);

            if (processExitArgs.ExitCode != 0)
                OnErrorDataReceived?.Invoke(this, new ProcessStdStreamEventArgs(processExitArgs.GetDescription()));


            OnProcessExit.Invoke(this, CreateProcessExitEventArgs(extractionProcess, isCanceled));
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

            OnProcessExit?.Invoke(this, CreateProcessExitEventArgs(extractionProcess, token.IsCancellationRequested));
            StopReading();
        }
#endif

    public void Compress(IArchive archive, WinRARCompressionSettings settings) {
        CompressInternal(archive.Name, settings, archive.FileNames.Concat(archive.DirectoryNames).ToArray());
    }

    public void Compress(Func<IArchiveBuilder, IArchive> archiveBuilder, WinRARCompressionSettings settings) {
        IArchive archive = archiveBuilder.Invoke(new ArchiveBuilder());
        CompressInternal(archive.Name, settings, archive.FileNames.Concat(archive.DirectoryNames).ToArray());
    }

#if NET5_0_OR_GREATER
    public Task CompressAsync(IArchive archive, WinRARCompressionSettings settings, CancellationToken token = default) {
        return CompressAsyncInternal(archive.Name, settings, archive.FileNames.Concat(archive.DirectoryNames), token);
    }

    public Task CompressAsync(Func<IArchiveBuilder, IArchive> archiveBuilder, WinRARCompressionSettings settings, CancellationToken token = default) {
        IArchive archive = archiveBuilder.Invoke(new ArchiveBuilder());
        return CompressAsync(archive, settings, token);
    }
#endif

    public void Compress(IArchive archive) {
        Compress(archive, WinRARCompressionSettings.Default);
    }

    public void Compress(Func<IArchiveBuilder, IArchive> archiveBuilder) {
        Compress(archiveBuilder, WinRARCompressionSettings.Default);
    }

    public void Compress(string sourceDirectory, string destinationArchive)
       => Compress(sourceDirectory, destinationArchive, WinRARCompressionSettings.Default);

    public void Extract(string sourceArchive, string destinationDirectory)
       => Extract(sourceArchive, destinationDirectory, new WinRARExtractionSettings());

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

        if (OnProcessExit != null) {
            bool isCanceled = ProcessTimeout.HasValue &&
                (compressionProcess.ExitTime - compressionProcess.StartTime).TotalMilliseconds >= ProcessTimeout;

            ProcessExitEventArgs processExitArgs = CreateProcessExitEventArgs(compressionProcess, isCanceled);

            if (processExitArgs.ExitCode != 0)
                OnErrorDataReceived?.Invoke(this, new ProcessStdStreamEventArgs(processExitArgs.GetDescription()));

            OnProcessExit.Invoke(this, processExitArgs);
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
        OnProcessExit?.Invoke(this, CreateProcessExitEventArgs(compressionProcess, token.IsCancellationRequested));
        StopReading();
    }
#endif

    private Process CreateProcess(string arguments, WinRARExecutableMode executableMode, bool compress) {
        ProcessStartInfo startInfo = new ProcessStartInfo {
            FileName = winRARPath + (executableMode == WinRARExecutableMode.WinRAR
            ? "\\WinRAR.exe"
            : (compress ? "\\Rar.exe" : "\\UnRAR.exe")),

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
            standardError!.AppendLine(e.Data);
            OnErrorDataReceived?.Invoke(sender, new ProcessStdStreamEventArgs(e.Data));
        }
        else {
            standardOutput!.AppendLine(e.Data);
            OnOutputDataReceived?.Invoke(sender, new ProcessStdStreamEventArgs(e.Data));
        }
    }

    private static ProcessExitEventArgs CreateProcessExitEventArgs(Process process, bool isCanceled) {
        return new ProcessExitEventArgs(process.ExitCode, process.StartTime, process.ExitTime, isCanceled);
    }

    private void StartReading() {
        standardOutput = new StringBuilder();
        standardError = new StringBuilder();
    }

    private void StopReading() {
        StdOutput = standardOutput!.ToString()!;
        StdError = standardError!.ToString()!;

        standardOutput = null;
        standardError = null;
    }
    #endregion
}
