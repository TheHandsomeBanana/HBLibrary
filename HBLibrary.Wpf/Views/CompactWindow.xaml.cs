using System.Windows;
using System.Windows.Input;

namespace HBLibrary.Wpf.Views {
    /// <summary>
    /// Interaction logic for CompactWindow.xaml
    /// </summary>
    public partial class CompactWindow : Window {
        public CompactWindow() {
            InitializeComponent();
        }

        // Can execute
        private void CommandBinding_CanExecute(object sender, CanExecuteRoutedEventArgs e) {
            e.CanExecute = true;
        }


        // Close
        private void CommandBinding_Executed_Close(object sender, ExecutedRoutedEventArgs e) {
            SystemCommands.CloseWindow(this);
        }

        protected override void OnContentRendered(EventArgs e) {
            base.OnContentRendered(e);

            // Content of window may be black in case of SizeToContent is set. 
            // This eliminates the problem. 
            // Do not use InvalidateVisual because it may implicitly break your markup.
            InvalidateMeasure();
        }
    }
}
