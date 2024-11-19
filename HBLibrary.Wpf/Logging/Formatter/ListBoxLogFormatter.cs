using HBLibrary.Interface.Logging;
using HBLibrary.Interface.Logging.Formatting;
using HBLibrary.Interface.Logging.Statements;
using HBLibrary.Wpf.Logging.Statements;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace HBLibrary.Wpf.Logging.Formatter;
public class ListBoxLogFormatter : ILogFormatter {
    public SolidColorBrush SuccessBrush { get; set; } = Brushes.MediumSeaGreen;
    public SolidColorBrush InfoBrush { get; set; } = Brushes.White;
    public SolidColorBrush WarningBrush { get; set; } = Brushes.Yellow;
    public SolidColorBrush ErrorBrush { get; set; } = Brushes.IndianRed;
    public SolidColorBrush CriticalBrush { get; set; } = Brushes.Red;

    public object Format(ILogStatement logStatement) {
        SolidColorBrush brush = logStatement.Level switch {
            LogLevel.Debug or LogLevel.Info => InfoBrush,
            LogLevel.Warning => WarningBrush,
            LogLevel.Error => ErrorBrush,
            LogLevel.Fatal => CriticalBrush,
            _ => InfoBrush,
        };


        return new ListBoxLog {
            ForegroundColor = brush,
            LogLevel = logStatement.Level?.ToString(),
            Message = logStatement.Message,
            Timestamp = logStatement.CreatedOn,
            OwnerCategory = logStatement.Name,
        };
    }
    
    public ListBoxLog FormatSuccess(ILogStatement logStatement) {
        SolidColorBrush brush = logStatement.Level switch {
            LogLevel.Debug or LogLevel.Info => SuccessBrush,
            LogLevel.Warning => WarningBrush,
            LogLevel.Error => ErrorBrush,
            LogLevel.Fatal => CriticalBrush,
            _ => InfoBrush,
        };


        return new ListBoxLog {
            ForegroundColor = brush,
            LogLevel = logStatement.Level?.ToString(),
            Message = logStatement.Message,
            Timestamp = logStatement.CreatedOn,
            OwnerCategory = logStatement.Name,
        };
    }

    public ListBoxLog FormatBlock(LogBlockStatement blockStatement) {
        // No Level, no Category, no color, simply a block
        return new ListBoxLog {
            Message = blockStatement.Message,
            Timestamp = blockStatement.CreatedOn,
            ForegroundColor = InfoBrush
        };
    }
}
