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
public class WinRARArchiver : IWinRARArchiver {
    private readonly string winRARInstallationPath;
    private StringBuilder? standardOutput;
    private StringBuilder? standardError;

    public event EventHandler<ProcessExitEventArgs>? OnProcessExit;
    public event EventHandler<ProcessStdStreamEventArgs>? OnOutputDataReceived;
    public event EventHandler<ProcessStdStreamEventArgs>? OnErrorDataReceived;

    public WinRARArchiver() {
        winRARInstallationPath = WinRARManager.GetWinRARPath()
            ?? throw new ApplicationNotFoundException("WinRAR", "If only the cli is available, add the 'Rar.exe' or 'UnRAR.exe' path to your 'PATH' environment variable.");
    }

    public WinRARCommandExecutionResult Execute(IWinRARCommand command, int timeout = Timeout.Infinite) {
        using Process process = CreateProcess(command.ToCommandString());
        StartReading();
        process.Start();
        process.WaitForExit(timeout);
        (string stdOutput, string stdError) = StopReading();

        bool isCanceled = (process.ExitTime - process.StartTime).TotalMilliseconds >= timeout;

        WinRARCommandExecutionResult result = new WinRARCommandExecutionResult() {
            ExitCode = process.ExitCode,
            ExitCodeMessage = WinRARCommandExecutionResult.GetDescription(process.ExitCode),
            StartTime = process.StartTime,
            EndTime = process.ExitTime,
            StdOutput = stdOutput,
            StdError = stdError,
            IsCanceled = isCanceled,
        };

        OnProcessExit?.Invoke(this, new ProcessExitEventArgs(process.ExitCode, process.StartTime, process.ExitTime, isCanceled));
        return result;
    }

    public WinRARCommandExecutionResult Execute(Func<IWinRARCommandProvider, IWinRARCommand> commandBuilder, int timeout = Timeout.Infinite) {
        IWinRARCommand command = commandBuilder.Invoke(new WinRARCommandProvider());
        return Execute(command, timeout);
    }

#if NET5_0_OR_GREATER
    public async Task<WinRARCommandExecutionResult> ExecuteAsync(IWinRARCommand command, CancellationToken token = default) {
        using Process process = CreateProcess(command.ToCommandString());
        StartReading();
        process.Start();
        await process.WaitForExitAsync(token);

        (string stdOutput, string stdError) = StopReading();

        WinRARCommandExecutionResult result = new WinRARCommandExecutionResult() {
            ExitCode = process.ExitCode,
            ExitCodeMessage = WinRARCommandExecutionResult.GetDescription(process.ExitCode),
            StartTime = process.StartTime,
            EndTime = process.ExitTime,
            StdOutput = stdOutput,
            StdError = stdError,
            IsCanceled = token.IsCancellationRequested,
        };

        OnProcessExit?.Invoke(this, new ProcessExitEventArgs(process.ExitCode, process.StartTime, process.ExitTime, token.IsCancellationRequested));
        return result;
    }

    public Task<WinRARCommandExecutionResult> ExecuteAsync(Func<IWinRARCommandProvider, IWinRARCommand> commandBuilder, CancellationToken token = default) {
        IWinRARCommand command = commandBuilder.Invoke(new WinRARCommandProvider());
        return ExecuteAsync(command, token);
    }
#endif

    private void OutputDataReceived(object sender, DataReceivedEventArgs e) {
        if (string.IsNullOrEmpty(e.Data))
            return;

        // WinRAR writes errors to stdout for some crazy idk reason
        //if (e.Data.StartsWith("error", StringComparison.CurrentCultureIgnoreCase)) {
        //    standardError!.AppendLine(e.Data);
        //    OnErrorDataReceived?.Invoke(sender, new ProcessStdStreamEventArgs(e.Data));
        //}
        //else {
        standardOutput!.AppendLine(e.Data);
        OnOutputDataReceived?.Invoke(sender, new ProcessStdStreamEventArgs(e.Data));
        //}
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

    private (string, string) StopReading() {
        (string, string) output = (standardOutput!.ToString(), standardError!.ToString());
        standardOutput = null;
        standardError = null;
        return output;
    }

    private Process CreateProcess(string commandString) {
        ProcessStartInfo startInfo = new ProcessStartInfo {
            FileName = winRARInstallationPath,
            Arguments = commandString,
            RedirectStandardInput = true,
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
        process.ErrorDataReceived += ErrorDataReceived;

        return process;
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