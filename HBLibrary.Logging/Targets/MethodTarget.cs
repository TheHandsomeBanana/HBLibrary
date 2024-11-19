using HBLibrary.Interface.Logging;
using HBLibrary.Interface.Logging.Configuration;
using HBLibrary.Interface.Logging.Formatting;
using HBLibrary.Interface.Logging.Statements;
using HBLibrary.Interface.Logging.Targets;
using HBLibrary.Logging.Configuration;

namespace HBLibrary.Logging.Targets;
public sealed class MethodTarget : ILogTarget, IEquatable<MethodTarget> {
    public LogLevel? LevelThreshold { get; }
    public LogStatementDelegate Method { get; private set; }
    public ILogFormatter? Formatter { get; }

    public MethodTarget(LogStatementDelegate method, LogLevel? minLevel = null, ILogFormatter? formatter = null) {
        LevelThreshold = minLevel;
        Method = method;
        Formatter = formatter;
    }

    public void WriteLog(ILogStatement log, ILogFormatter? formatter) => Method?.Invoke(log, formatter);

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
