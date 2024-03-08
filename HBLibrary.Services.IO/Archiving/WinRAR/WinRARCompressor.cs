using HBLibrary.Common.Process;
using HBLibrary.Services.IO.Archiving.WinRAR.Options;
using System.Diagnostics;

namespace HBLibrary.Services.IO.Archiving.WinRAR;
public class WinRARCompressor : WinRARArchiverBase, IWinRARCompressor {
    public void Compress(Archive archive) {
        Compress(archive, WinRARCompressionOptions.Default);
    }

    public void Compress(Archive archive, WinRARCompressionOptions settings) {
        StartReading();

        string destinationArchive = settings.SetExtension(archive.Name);

        string arguments = settings.ToString()! + $"\"{destinationArchive}\" {string.Join(" ", archive.GetEntries().Select(e => $"\"{e}\""))}";
        using Process compressionProcess = CreateProcess(arguments, true);
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
    public async Task CompressAsync(Archive archive, WinRARCompressionOptions settings, CancellationToken token = default) {
        StartReading();

        string destinationArchive = settings.SetExtension(archive.Name);

        string arguments = settings.ToString()! + $"\"{destinationArchive}\" {string.Join(" ", archive.GetEntries().Select(e => $"\"{e}\""))}";
        using Process compressionProcess = CreateProcess(arguments, true);
        compressionProcess.Start();

        compressionProcess.BeginErrorReadLine();
        compressionProcess.BeginOutputReadLine();

        await compressionProcess.WaitForExitAsync(token);
        RaiseOnProcessExit(CreateProcessExitEventArgs(compressionProcess, token.IsCancellationRequested));
        StopReading();
    }
#endif 
}
