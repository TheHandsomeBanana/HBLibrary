using System;
using System.Collections.Generic;
using System.Linq;
using System.Media;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace HBLibrary.Wpf.Views;
/// <summary>
/// Interaction logic for HBDarkMessageBox.xaml
/// </summary>
public partial class HBDarkMessageBox : Window {
    public HBDarkMessageBox(string title, string message) {
        InitializeComponent();
        txbTitle.Text = title;
        txbMessage.Text = message;
        SystemSounds.Exclamation.Play();
    }

    public static bool Show(string title, string message) {
        HBDarkMessageBox messageBox = new HBDarkMessageBox(title, message);
        return messageBox.ShowDialog() ?? false;
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

    private void Button_Click(object sender, RoutedEventArgs e) {
        SystemCommands.CloseWindow(this);
    }
}
