using HBLibrary.Interface.Logging;
using HBLibrary.Interface.Logging.Configuration;
using HBLibrary.Interface.Logging.Exceptions;
using HBLibrary.Interface.Logging.Formatting;
using HBLibrary.Interface.Logging.Statements;
using HBLibrary.Interface.Logging.Targets;
using HBLibrary.Wpf.Logging.Formatter;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace HBLibrary.Wpf.Logging;
public class ListBoxLogTarget : ILogTarget {
    public LogLevel? LevelThreshold { get; set; }
    public ObservableCollection<ListBoxLog> Logs { get; set; } = [];

    public void WriteLog(LogStatement log, ILogFormatter? formatter = null) {
        formatter ??= new ListBoxLogFormatter();

        if (formatter.Format(log) is not ListBoxLog formatted) {
            throw new LoggingException($"The formatter does not return the requested type {nameof(ListBoxLog)}");
        }

        Application.Current.Dispatcher.Invoke(() => {
            Logs.Add(formatted);
        });
    }

    public void WriteSuccessLog(LogStatement logStatement) {
        ListBoxLogFormatter formatter = new ListBoxLogFormatter();
        ListBoxLog log = formatter.FormatSuccess(logStatement);

        Application.Current.Dispatcher.Invoke(() => {
            Logs.Add(log);
        });
    }


    public void Dispose() {
    }
}
