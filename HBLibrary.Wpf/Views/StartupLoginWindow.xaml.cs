using System.Windows;

namespace HBLibrary.Wpf.Views {
    /// <summary>
    /// Interaction logic for StartupLoginWindow.xaml
    /// </summary>
    public partial class StartupLoginWindow : Window {
        public StartupLoginWindow() {
            InitializeComponent();
        }


        protected override void OnContentRendered(EventArgs e) {
            base.OnContentRendered(e);

            // Content of window may be black in case of SizeToContent is set. 
            // This eliminates the problem. 
            // Do not use InvalidateVisual because it may implicitly break your markup.
            InvalidateMeasure();
        }

        private void RegisterButton_Checked(object sender, RoutedEventArgs e) {
            tbLogin.IsChecked = false;
        }

        private void LoginButton_Checked(object sender, RoutedEventArgs e) {
            if (tbRegister is not null) {
                tbRegister.IsChecked = false;
            }
        }
    }
}
