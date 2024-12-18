﻿namespace HBLibrary.Core.Process;
public class ProcessExitEventArgs : EventArgs {
    public int ExitCode { get; }
    public bool IsCanceled { get; }
    public DateTime StartTime { get; }
    public DateTime EndTime { get; }
    public TimeSpan Duration => EndTime - StartTime;

    public ProcessExitEventArgs(int exitCode, DateTime startTime, DateTime endTime, bool wasCanceled) {
        ExitCode = exitCode;
        StartTime = startTime;
        EndTime = endTime;
        IsCanceled = wasCanceled;
    }
}
