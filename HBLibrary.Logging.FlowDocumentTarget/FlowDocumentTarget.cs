using HBLibrary.Interface.Logging;
using HBLibrary.Interface.Logging.Configuration;
using HBLibrary.Interface.Logging.Formatting;
using HBLibrary.Interface.Logging.Statements;
using HBLibrary.Interface.Logging.Targets;
using HBLibrary.Logging.FlowDocumentTarget.Formatter;
using System.ComponentModel;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Media;

namespace HBLibrary.Logging.FlowDocumentTarget;

public class FlowDocumentTarget : ILogTarget, INotifyPropertyChanged {
    // Required to keep track of the statements for serialization / deserialization
    private readonly List<LogWithMetadata> statements = [];
    public IReadOnlyList<LogWithMetadata> Statements => statements;

    public LogLevel? LevelThreshold { get; }

    public event PropertyChangedEventHandler? PropertyChanged;
    public FlowDocument? Document { get; private set; } 
    public Thickness ParagraphMargin { get; set; } = new Thickness(0);


    public FlowDocumentTarget() {
        // Make sure the Document is part of the UI Thread
        Application.Current.Dispatcher.Invoke(() => {
            Document = new FlowDocument();
        });
    }

    public Brush InfoBrush { get; set; } = Brushes.White;
    public Brush WarningBrush { get; set; } = Brushes.Yellow;
    public Brush ErrorBrush { get; set; } = Brushes.IndianRed;
    public Brush CriticalBrush { get; set; } = Brushes.Red;

    public void WriteLog(LogStatement log, ILogFormatter? formatter = null) {
        formatter ??= new MessageOnlyFormatter();

        Brush brush = log.Level switch {
            LogLevel.Debug or LogLevel.Info => InfoBrush,
            LogLevel.Warning => WarningBrush,
            LogLevel.Error => ErrorBrush,
            LogLevel.Fatal => CriticalBrush,
            _ => InfoBrush,
        };

        Application.Current.Dispatcher.Invoke(() => {

            Document!.Blocks.Add(new Paragraph(new Run((string)formatter.Format(log))) {
                TextIndent = 5,
                FontFamily = new FontFamily("Consolas"),
                FontSize = 14,
                Margin = ParagraphMargin,
                Foreground = brush
            });

            NotifyDocumentChanged();
        });

        statements.Add(new LogWithMetadata {
            Log = log,
            IsSuccess = false
        });
    }

    public void WriteSuccessLog(LogStatement log, ILogFormatter? formatter = null) {
        formatter ??= new MessageOnlyFormatter();

        Brush brush = log.Level switch {
            LogLevel.Debug or LogLevel.Info => Brushes.MediumSeaGreen,
            LogLevel.Warning => WarningBrush,
            LogLevel.Error => ErrorBrush,
            LogLevel.Fatal => CriticalBrush,
            _ => InfoBrush,
        };

        Application.Current.Dispatcher.Invoke(() => {

            Document!.Blocks.Add(new Paragraph(new Run((string)formatter.Format(log))) {
                TextIndent = 5,
                FontFamily = new FontFamily("Consolas"),
                FontSize = 14,
                Margin = ParagraphMargin,
                Foreground = brush
            });

            NotifyDocumentChanged();
        });

        statements.Add(new LogWithMetadata {
            Log = log,
            IsSuccess = true
        });
    }


    public void NotifyDocumentChanged() {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Document)));
    }

    public void Dispose() { }
}
