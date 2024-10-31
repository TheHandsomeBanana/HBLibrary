using HBLibrary.Wpf.ViewModels;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Media;

namespace HBLibrary.Wpf {
    public class LogFlowDocument : ViewModelBase {
        public Thickness ParagraphMargin { get; set; } = new Thickness(0);
        public FlowDocument Document { get; } = new FlowDocument();

        public Brush SuccessBrush { get; set; } = Brushes.MediumSeaGreen;
        public Brush InfoBrush { get; set; } = Brushes.White;
        public Brush WarningBrush { get; set; } = Brushes.Yellow;
        public Brush ErrorBrush { get; set; } = Brushes.IndianRed;
        public Brush CriticalBrush { get; set; } = Brushes.Red;

        public void AddParagraph(Paragraph paragraph) {
            Application.Current.Dispatcher.Invoke(() => {
                Document.Blocks.Add(paragraph);
                NotifyDocumentChanged();
            });
        }

        public Paragraph AddParagraph(string message, Brush brush) {
            Paragraph paragraph = Application.Current.Dispatcher.Invoke<Paragraph>(() => {
                paragraph = new Paragraph(new Run(message)) {
                    TextAlignment = TextAlignment.Left,
                    Foreground = brush,
                    Margin = ParagraphMargin,
                    
                };

                Document.Blocks.Add(paragraph);
                NotifyDocumentChanged();
                return paragraph;
            });

            return paragraph;
        }

        public Paragraph AddSuccessParagraph(string message) {
            return AddParagraph(message, SuccessBrush);
        }

        public Paragraph AddInfoParagraph(string message) {
            return AddParagraph(message, InfoBrush);
        }

        public Paragraph AddWarningParagraph(string message) {
            return AddParagraph(message, WarningBrush);
        }

        public Paragraph AddErrorParagraph(string message) {
            return AddParagraph(message, ErrorBrush);
        }

        public Paragraph AddCriticalParagraph(string message) {
            return AddParagraph(message, CriticalBrush);
        }

        public void Clear() {
            Document.Blocks.Clear();
            NotifyDocumentChanged();
        }

        public void NotifyDocumentChanged() {
            NotifyPropertyChanged(nameof(Document));
        }
    }
}
