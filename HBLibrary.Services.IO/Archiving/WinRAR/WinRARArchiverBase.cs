using HBLibrary.Common.Process;
using HBLibrary.Services.IO.Compression.WinRAR;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HBLibrary.Services.IO.Archiving.WinRAR;
public abstract class WinRARArchiverBase {
    public event EventHandler<ProcessExitEventArgs>? OnProcessExit;
    public event EventHandler<ProcessStdStreamEventArgs>? OnOutputDataReceived;
    public event EventHandler<ProcessStdStreamEventArgs>? OnErrorDataReceived;
    public int? ProcessTimeout { get; set; }
    public string StdOutput { get; protected set; } = "";
    public string StdError { get; protected set; } = "";

    protected string WinRARPath { get; }
    protected StringBuilder? StandardOutput { get; set; }
    protected StringBuilder? StandardError { get; set; }


    public WinRARArchiverBase() {
        WinRARPath = WinRARHelper.GetWinRARInstallationPath()
            ?? throw new DirectoryNotFoundException("Could not detect a WinRAR installation.");
    }

    protected bool OnProcessExitIsNull() => OnProcessExit == null;
    protected bool OnOutputDataReceivedIsNull() => OnOutputDataReceived == null;
    protected bool OnErrorDataReceivedIsNull() => OnErrorDataReceived == null;

    protected virtual void RaiseOnProcessExit(ProcessExitEventArgs e) {
        OnProcessExit?.Invoke(this, e);
    }

    protected virtual void RaiseOnOutputDataReceived(ProcessStdStreamEventArgs e) {
        OnOutputDataReceived?.Invoke(this, e);
    }

    protected virtual void RaiseOnOutputDataReceived(object sender, ProcessStdStreamEventArgs e) {
        OnOutputDataReceived?.Invoke(sender, e);
    }

    protected virtual void RaiseOnErrorDataReceived(ProcessStdStreamEventArgs e) {
        OnErrorDataReceived?.Invoke(this, e);
    }

    protected virtual void RaiseOnErrorDataReceived(object sender, ProcessStdStreamEventArgs e) {
        OnErrorDataReceived?.Invoke(sender, e);
    }

    protected static ProcessExitEventArgs CreateProcessExitEventArgs(Process process, bool isCanceled) {
        return new ProcessExitEventArgs(process.ExitCode, process.StartTime, process.ExitTime, isCanceled);
    }

    protected void StartReading() {
        StandardOutput = new StringBuilder();
        StandardError = new StringBuilder();
    }

    protected void StopReading() {
        StdOutput = StandardOutput!.ToString()!;
        StdError = StandardError!.ToString()!;

        StandardOutput = null;
        StandardError = null;
    }
}
