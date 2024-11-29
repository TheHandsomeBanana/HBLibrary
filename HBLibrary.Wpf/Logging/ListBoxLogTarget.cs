using HBLibrary.Interface.Logging;
using HBLibrary.Interface.Logging.Configuration;
using HBLibrary.Interface.Logging.Exceptions;
using HBLibrary.Interface.Logging.Formatting;
using HBLibrary.Interface.Logging.Statements;
using HBLibrary.Interface.Logging.Targets;
using HBLibrary.Wpf.Logging.Formatter;
using HBLibrary.Wpf.Logging.Statements;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

namespace HBLibrary.Wpf.Logging;
public sealed class ListBoxLogTarget : IExtendedLogTarget<ListBoxLog> {


    public LogLevel? LevelThreshold { get; set; }
    public ObservableCollection<ListBoxLog> Logs { get; set; } = [];
    [JsonIgnore]
    public ILogFormatter? Formatter { get; }

    [JsonConstructor]
    public ListBoxLogTarget() {

    }

    public ListBoxLogTarget(LogLevel? minLevel = null, ILogFormatter? formatter = null) : this() {
        this.LevelThreshold = minLevel;
        this.Formatter = formatter;
    }

    public int WriteIndexedLog(ILogStatement log, ILogFormatter? formatter = null) {
        formatter ??= new ListBoxLogFormatter();

        if (formatter.Format(log) is not ListBoxLog formatted) {
            throw new LoggingException($"The formatter does not return the requested type {nameof(ListBoxLog)}");
        }

        return Application.Current.Dispatcher.Invoke(() => {
            Logs.Add(formatted);
            return Logs.IndexOf(formatted);
        });
    }

    public void WriteLog(ILogStatement log, ILogFormatter? formatter = null) {
        formatter ??= new ListBoxLogFormatter();

        if (formatter.Format(log) is not ListBoxLog formatted) {
            throw new LoggingException($"The formatter does not return the requested type {nameof(ListBoxLog)}");
        }

        Application.Current.Dispatcher.Invoke(() => Logs.Add(formatted));
    }

    public void WriteSuccessLog(ILogStatement logStatement) {
        ListBoxLogFormatter formatter = new ListBoxLogFormatter();
        ListBoxLog log = formatter.FormatSuccess(logStatement);

        Application.Current.Dispatcher.Invoke(() => Logs.Add(log));

    }

    public void WriteLogBlock(LogBlockStatement logBlock) {
        ListBoxLogFormatter formatter = new ListBoxLogFormatter();
        ListBoxLog log = formatter.FormatBlock(logBlock);

        Application.Current.Dispatcher.Invoke(() => Logs.Add(log));
    }

    public void RewriteLog(int logIndex, string message) {
        Application.Current.Dispatcher.Invoke(() => {
            ListBoxLog log = GetLog(logIndex);
            // Message is bound -> Changes are reflected to UI
            log.Message = message; 
        });
    }

    public ListBoxLog[] GetLogs() {
        return [.. Logs];
    }

    public IEnumerable<ListBoxLog> EnumerateLogs() {
        return Logs;
    }

    public ListBoxLog GetLog(int index) {
        return Logs[index];
    }

    public void Dispose() {

    }

    #region untyped
    object[] IExtendedLogTarget.GetLogs() {
        return GetLogs();
    }

    IEnumerable<object> IExtendedLogTarget.EnumerateLogs() {
        return EnumerateLogs();
    }

    object IExtendedLogTarget.GetLog(int index) {
        return GetLog(index);
    }
    #endregion
}
