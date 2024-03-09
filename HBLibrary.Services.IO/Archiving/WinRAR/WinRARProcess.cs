using HBLibrary.Common.Exceptions;
using HBLibrary.Common.Process;
using HBLibrary.Services.IO.Archiving.WinRAR.Commands;
using HBLibrary.Services.IO.Archiving.WinRAR.Commands.Builder;
using HBLibrary.Services.IO.Exceptions;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace HBLibrary.Services.IO.Archiving.WinRAR;
public class WinRARProcess : IDisposable {
    private bool disposedValue;
    private readonly string winRARInstallationPath;
    private readonly Process process;
    private StringBuilder? standardOutput;
    private StringBuilder? standardError;

    public event EventHandler<ProcessExitEventArgs>? OnProcessExit;
    public event EventHandler<ProcessStdStreamEventArgs>? OnOutputDataReceived;
    public event EventHandler<ProcessStdStreamEventArgs>? OnErrorDataReceived;

    /// <summary>
    /// Contains all written outputs, available after the process has exited.
    /// </summary>
    public string StdOutput { get; protected set; } = "";
    /// <summary>
    /// Containers all written errors, available after the process has exited.
    /// </summary>
    public string StdError { get; protected set; } = "";

   

    public WinRARProcess(WinRARProcessMode processMode) {
        winRARInstallationPath = WinRARHelper.GetWinRARInstallationPath()
            ?? throw new ApplicationNotFoundException("WinRAR");

        ProcessStartInfo startInfo = new ProcessStartInfo {
            FileName = winRARInstallationPath + (processMode == WinRARProcessMode.Rar ? "\\Rar.exe" : "\\UnRAR.exe"),

            RedirectStandardInput = true,
            RedirectStandardError = true,
            RedirectStandardOutput = true,
            CreateNoWindow = true,
            UseShellExecute = false,
        };

        process = new Process {
            EnableRaisingEvents = true,
            StartInfo = startInfo
        };

        process.OutputDataReceived += OutputDataReceived;
        process.ErrorDataReceived += ErrorDataReceived;
    }
    
    public void Start() {
        StartReading();
        process.Start();
        process.BeginErrorReadLine();
        process.BeginOutputReadLine();
    }

    public void ExecuteCommand(WinRARCommand command) {
        if (!process.StandardInput.BaseStream.CanWrite)
            throw new WinRARException("Cannot execute command, cannot write to standard input.");

        process.StandardInput.WriteLine(command.ToCommandString());
    }

    public void ExecuteCommand(Func<IWinRARCommandBuilder, WinRARCommand> commandBuilder) {
        WinRARCommand command = commandBuilder.Invoke(new WinRARCommandBuilder());
        ExecuteCommand(command);
    }

    public bool WaitForExit(int timeout = Timeout.Infinite) {
        bool exit = process.WaitForExit(timeout);
        StopReading();
        return exit;
    }

#if NET5_0_OR_GREATER
    public async Task WaitForExitAsync(CancellationToken cancellationToken = default) {
        await process.WaitForExitAsync(cancellationToken);
        StopReading();
    }
#endif

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

    private void ErrorDataReceived(object sender, DataReceivedEventArgs e) {
        if (string.IsNullOrEmpty(e.Data))
            return;

        standardError!.AppendLine(e.Data);
        OnErrorDataReceived?.Invoke(sender, new ProcessStdStreamEventArgs(e.Data));
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

    protected virtual void Dispose(bool disposing) {
        if (!disposedValue) {
            if (disposing) {
                process.Dispose();
            }

            // TODO: free unmanaged resources (unmanaged objects) and override finalizer
            // TODO: set large fields to null
            disposedValue = true;
        }
    }

    ~WinRARProcess() {
        Dispose(disposing: false);
    }

    public void Dispose() {
        Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }
}

public enum WinRARProcessMode {
    /// <summary>
    /// When you need to create a new archive, modify an existing archive, or perform any advanced archive management tasks.
    /// <br></br>Can perform all UnRAR commands and the following:<br/>
    /// Creating archives (a)<br/>
    /// Updating archives (u)<br/>
    /// Deleting files from archive (d)<br/>
    /// Freshen files in archive (f)<br/>
    /// Printing file to Stdout (p)<br/>
    /// Converting archive formats (ch)<br/>
    /// Locking archives (k)<br/>
    /// Adding recovery records (rr)<br/>
    /// Creating SFX archives (s)<br/>
    /// Setting advanced compression parameters (-m, -md)<br/>
    /// Volume splitting (-v)<br/>
    /// Commenting (c)<br/>
    /// Protecting archives (-hp)<br/>
    /// </summary>
    Rar,
    /// <summary>
    /// Specifically designed for extracting RAR archives and is freely distributable
    /// under a license that allows it to be used in a variety of contexts, 
    /// including commercially and in open-source projects
    /// </summary>
    UnRAR
}