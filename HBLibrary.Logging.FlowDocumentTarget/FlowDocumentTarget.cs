using HBLibrary.Interface.Logging;
using HBLibrary.Interface.Logging.Configuration;
using HBLibrary.Interface.Logging.Statements;
using HBLibrary.Interface.Logging.Targets;
using System.ComponentModel;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Media;

namespace HBLibrary.Logging.FlowDocumentTarget;

public class FlowDocumentTarget : ILogTarget, INotifyPropertyChanged {
    public LogLevel? LevelThreshold { get; set; }

    public event PropertyChangedEventHandler? PropertyChanged;
    public FlowDocument Document { get; } = new FlowDocument();
    public Thickness ParagraphMargin { get; set; } = new Thickness(0);

    public Brush InfoBrush { get; set; } = Brushes.White;
    public Brush WarningBrush { get; set; } = Brushes.Yellow;
    public Brush ErrorBrush { get; set; } = Brushes.IndianRed;
    public Brush CriticalBrush { get; set; } = Brushes.Red;

    public void WriteLog(LogStatement log, LogDisplayFormat displayFormat = LogDisplayFormat.Full) {
        Brush brush = log.Level switch {
            LogLevel.Debug or LogLevel.Info => InfoBrush,
            LogLevel.Warning => WarningBrush,
            LogLevel.Error => ErrorBrush,
            LogLevel.Fatal => CriticalBrush,
            _ => InfoBrush,
        };

        Application.Current.Dispatcher.Invoke(() => {

            Document.Blocks.Add(new Paragraph(new Run(log.Format(displayFormat))) {
                TextIndent = 10,
                Margin = ParagraphMargin,
                Foreground = brush
            });

            NotifyDocumentChanged();
        });
    }

    public void WriteSuccessLog(LogStatement log, LogDisplayFormat displayFormat = LogDisplayFormat.Full) {
        Brush brush = Brushes.MediumSeaGreen;

        Application.Current.Dispatcher.Invoke(() => {

            Document.Blocks.Add(new Paragraph(new Run(log.Format(displayFormat))) {
                TextIndent = 10,
                Margin = ParagraphMargin,
                Foreground = brush
            });

            NotifyDocumentChanged();
        });
    }


    public void NotifyDocumentChanged() {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Document)));
    }

    public void Dispose() { }
}
