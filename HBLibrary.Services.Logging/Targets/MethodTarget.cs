using HBLibrary.Services.Logging.Configuration;
using HBLibrary.Services.Logging.Statements;

namespace HBLibrary.Services.Logging.Targets;
public sealed class MethodTarget : ILogTarget, IEquatable<MethodTarget> {
    public LogLevel? LevelThreshold { get; }
    public LogStatementDelegate Method { get; private set; }

    public MethodTarget(LogStatementDelegate method, LogLevel? minLevel = null) {
        LevelThreshold = minLevel;
        Method = method;
    }

    public void WriteLog(LogStatement log, LogDisplayFormat format = LogDisplayFormat.Full) => Method?.Invoke(log, format);

    public void Dispose() {
#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.
        Method = null;
#pragma warning restore CS8625 // Cannot convert null literal to non-nullable reference type.
    }

    public bool Equals(MethodTarget? other) => Method == other?.Method;

    public override bool Equals(object? obj) {
        return obj is MethodTarget mt && Equals(mt);
    }

    public override int GetHashCode() {
        return Method.GetHashCode();
    }

    public static bool operator ==(MethodTarget a, MethodTarget b) => a.Equals(b);
    public static bool operator !=(MethodTarget a, MethodTarget b) => !(a == b);
}
