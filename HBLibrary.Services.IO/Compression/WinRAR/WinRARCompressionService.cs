using HBLibrary.Common.Process;
using HBLibrary.Services.IO.Exceptions;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HBLibrary.Services.IO.Compression.WinRAR {
    public class WinRARCompressionService : IWinRARCompressionService {
        private readonly string winRARPath;

        public event EventHandler<ProcessExitEventArgs>? OnProcessExit;
        public event DataReceivedEventHandler? OnErrorDataReceived;
        public event DataReceivedEventHandler? OnOutputDataReceived;

        public int? ProcessTimeout { get; set; }

        public WinRARCompressionService() {
            this.winRARPath = WinRARHelper.GetWinRARInstallationPath()
                ?? throw new DirectoryNotFoundException("Could not detect a WinRAR installation.");
        }

        public void Compress(string source, string destinationArchive, WinRARCompressionSettings settings) {
            string arguments = settings.ToString()! + $"\"{destinationArchive}\" \"{source}\"";
            using Process compressionProcess = CreateProcess(arguments);
            compressionProcess.Start();
            compressionProcess.WaitForExit(ProcessTimeout ?? Timeout.Infinite);

            if (OnProcessExit != null) {
                bool isCanceled = ProcessTimeout.HasValue &&
                    (compressionProcess.ExitTime - compressionProcess.StartTime).TotalMilliseconds >= ProcessTimeout;

                OnProcessExit.Invoke(this, CreateProcessExitEventArgs(compressionProcess, isCanceled));
            }
        }

        public void Extract(string sourceArchive, string destinationDirectory, WinRARExtractionSettings settings) {
            string arguments = settings.ToString()! + $"\"{sourceArchive}\" \"{destinationDirectory}\"";
            using Process extractionProcess = CreateProcess(arguments);
            extractionProcess.Start();
            extractionProcess.WaitForExit(ProcessTimeout ?? Timeout.Infinite);

            if(OnProcessExit != null) {
                bool isCanceled = ProcessTimeout.HasValue && 
                    (extractionProcess.ExitTime - extractionProcess.StartTime).TotalMilliseconds >= ProcessTimeout;

                OnProcessExit.Invoke(this, CreateProcessExitEventArgs(extractionProcess, isCanceled));
            }
        }

#if NET5_0_OR_GREATER
        public async Task CompressAsync(string source, string destinationArchive, WinRARCompressionSettings settings, CancellationToken token = default) {
            string arguments = settings.ToString()! + $"\"{destinationArchive}\" \"{source}\"";
            using Process compressionProcess = CreateProcess(arguments);
            compressionProcess.Start();
            await compressionProcess.WaitForExitAsync(token);
            OnProcessExit?.Invoke(this, CreateProcessExitEventArgs(compressionProcess, token.IsCancellationRequested));
        }

        public async Task ExtractAsync(string sourceArchive, string destinationDirectory, WinRARExtractionSettings settings, CancellationToken token = default) {
            string arguments = settings.ToString()! + $"\"{sourceArchive}\" \"{destinationDirectory}\"";
            using Process extractionProcess = CreateProcess(arguments);
            extractionProcess.Start();
            await extractionProcess.WaitForExitAsync(token);

            OnProcessExit?.Invoke(this, CreateProcessExitEventArgs(extractionProcess, token.IsCancellationRequested));
        }
#endif

        public void CompressDirectory(string sourceDirectory, string destinationArchive)
           => Compress(sourceDirectory, destinationArchive, new WinRARCompressionSettings());

        public void CompressFile(string sourceFile, string destinationArchive)
           => Compress(sourceFile, destinationArchive, new WinRARCompressionSettings());

        public void Extract(string sourceArchive, string destinationDirectory)
           => Extract(sourceArchive, destinationDirectory, new WinRARExtractionSettings());

        private Process CreateProcess(string arguments) {

            ProcessStartInfo startInfo = new ProcessStartInfo {
                FileName = winRARPath + "\\WinRAR.exe",
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

            process.ErrorDataReceived += OnErrorDataReceived;
            process.OutputDataReceived += OnOutputDataReceived;

            return process;
        }

        private static ProcessExitEventArgs CreateProcessExitEventArgs(Process process, bool isCanceled) {
            return new ProcessExitEventArgs(process.ExitCode, process.StartTime, process.ExitTime, isCanceled);
        }
    }
}
