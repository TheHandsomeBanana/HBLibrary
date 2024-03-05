using HBLibrary.Common.Process;
using HBLibrary.Services.IO.Archiving.WinRAR.Options;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HBLibrary.Services.IO.Archiving.WinRAR;
public class WinRARExtractor : WinRARArchiverBase, IWinRARExtractor {
    public void Extract(FileSnapshot sourceArchive, DirectorySnapshot destinationDirectory, WinRARExtractionOptions settings) {
        StartReading();

        string arguments = settings.ToString()! + $"\"{sourceArchive}\" \"{destinationDirectory}\"";
        using Process extractionProcess = CreateProcess(arguments, false);
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

    public void Extract(FileSnapshot sourceArchive, DirectorySnapshot destinationDirectory) {
        Extract(sourceArchive, destinationDirectory, WinRARExtractionOptions.Default);
    }
#if NET5_0_OR_GREATER

    public async Task ExtractAsync(FileSnapshot sourceArchive, DirectorySnapshot destinationDirectory, WinRARExtractionOptions settings, CancellationToken token = default) {
        StartReading();

        string arguments = settings.ToString()! + $"\"{sourceArchive}\" \"{destinationDirectory}\"";
        using Process extractionProcess = CreateProcess(arguments, false);
        extractionProcess.Start();

        extractionProcess.BeginErrorReadLine();
        extractionProcess.BeginOutputReadLine();

        await extractionProcess.WaitForExitAsync(token);

        RaiseOnProcessExit(CreateProcessExitEventArgs(extractionProcess, token.IsCancellationRequested));
        StopReading();
    }
#endif

}
