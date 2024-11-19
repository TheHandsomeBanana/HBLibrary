using HBLibrary.Interface.Logging;
using HBLibrary.Interface.Logging.Configuration;
using HBLibrary.Interface.Logging.Formatting;
using HBLibrary.Interface.Logging.Statements;
using HBLibrary.Interface.Logging.Targets;
using HBLibrary.Logging.Configuration;

namespace HBLibrary.Logging.Targets;
public sealed class AsyncMethodTarget : IAsyncLogTarget, IEquatable<AsyncMethodTarget> {
    public AsyncLogStatementDelegate? Method { get; private set; }
    public LogLevel? LevelThreshold { get; set; }
    public ILogFormatter? Formatter { get; }

    public AsyncMethodTarget(AsyncLogStatementDelegate method, LogLevel? minLevel = null, ILogFormatter? formatter = null) {
        Method = method;
        LevelThreshold = minLevel;
    }

    public Task WriteLogAsync(ILogStatement log, ILogFormatter? formatter = null)
        => Method?.Invoke(log, formatter ?? Formatter)
        ?? Task.CompletedTask;

    public void Dispose() {
        Method = null;
    }

    public bool Equals(AsyncMethodTarget? other) => Method == other?.Method;
    public override bool Equals(object? obj) {
        return obj is AsyncMethodTarget amt && Equals(amt);
    }

    public override int GetHashCode() {
        return Method?.GetHashCode() ?? 0;
    }


    public static bool operator ==(AsyncMethodTarget a, AsyncMethodTarget b) => a.Equals(b);
    public static bool operator !=(AsyncMethodTarget a, AsyncMethodTarget b) => !(a == b);
}
