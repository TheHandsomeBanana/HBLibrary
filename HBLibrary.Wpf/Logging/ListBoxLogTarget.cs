using HBLibrary.Interface.Logging;
using HBLibrary.Interface.Logging.Configuration;
using HBLibrary.Interface.Logging.Exceptions;
using HBLibrary.Interface.Logging.Formatting;
using HBLibrary.Interface.Logging.Statements;
using HBLibrary.Interface.Logging.Targets;
using HBLibrary.Wpf.Logging.Formatter;
using HBLibrary.Wpf.Logging.Statements;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using System.Windows;

namespace HBLibrary.Wpf.Logging;
public class ListBoxLogTarget : ILogTarget {
    public LogLevel? LevelThreshold { get; set; }
    public ObservableCollection<ListBoxLog> Logs { get; set; } = [];
    [JsonIgnore]
    public ILogFormatter? Formatter { get; }

    public ListBoxLogTarget(LogLevel? minLevel = null, ILogFormatter? formatter = null) {
        this.LevelThreshold = minLevel;
        this.Formatter = formatter;
    }

    [JsonConstructor]
    public ListBoxLogTarget() {

    }

    public void WriteLog(ILogStatement log, ILogFormatter? formatter = null) {
        formatter ??= new ListBoxLogFormatter();

        if (formatter.Format(log) is not ListBoxLog formatted) {
            throw new LoggingException($"The formatter does not return the requested type {nameof(ListBoxLog)}");
        }

        Application.Current.Dispatcher.Invoke(() => {
            Logs.Add(formatted);
        });
    }

    public void WriteSuccessLog(ILogStatement logStatement) {
        ListBoxLogFormatter formatter = new ListBoxLogFormatter();
        ListBoxLog log = formatter.FormatSuccess(logStatement);

        Application.Current.Dispatcher.Invoke(() => {
            Logs.Add(log);
        });
    }

    public void WriteLogBlock(LogBlockStatement logBlock) {
        ListBoxLogFormatter formatter = new ListBoxLogFormatter();
        ListBoxLog log = formatter.FormatBlock(logBlock);

        Application.Current.Dispatcher.Invoke(() => {
            Logs.Add(log);
        });
    }

    public void Dispose() {
    }
}
