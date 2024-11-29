using HBLibrary.Interface.Logging.Formatting;
using HBLibrary.Interface.Logging.Statements;
using HBLibrary.Interface.Logging.Targets;
using Microsoft.Xaml.Behaviors.Layout;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HBLibrary.Wpf.Logging;
public interface IExtendedLogTarget : ILogTarget {
    public object[] GetLogs();
    public IEnumerable<object> EnumerateLogs();
    public object GetLog(int index);
    /// <summary>
    /// Writes the provided <paramref name="log"/> to this target, formatted using <paramref name="formatter"/>.
    /// If no specific formatter is passed, the default formatter is used.
    /// <br>The written log is accessible through the returned index</br>
    /// </summary>
    /// <param name="log"></param>
    /// <param name="formatter"></param>
    int WriteIndexedLog(ILogStatement log, ILogFormatter? formatter = null);

    void RewriteLog(int logIndex, string message);
}

public interface IExtendedLogTarget<TLogType> : IExtendedLogTarget {
    public new TLogType[] GetLogs();
    public new IEnumerable<TLogType> EnumerateLogs();
    public new TLogType GetLog(int index);
    /// <summary>
    /// Writes the provided <paramref name="log"/> to this target, formatted using <paramref name="formatter"/>.
    /// If no specific formatter is passed, the default formatter is used.
    /// <br>The written log is accessible through the returned index</br>
    /// </summary>
    /// <param name="log"></param>
    /// <param name="formatter"></param>
    public new int WriteIndexedLog(ILogStatement log, ILogFormatter? formatter = null);

    public new void RewriteLog(int logIndex, string message);
}