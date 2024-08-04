using System;
using System.Collections.Generic;
using System.Linq;
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
/// Interaction logic for HBDarkAccountWindow.xaml
/// </summary>
public partial class HBDarkAccountWindow : Window {
    public HBDarkAccountWindow() {
        InitializeComponent();
    }

    public HBDarkAccountWindow(Window owner) {
        this.Owner = owner;
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
