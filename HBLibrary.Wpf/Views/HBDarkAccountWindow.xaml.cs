using HBLibrary.Wpf.ViewModels;
using System.Windows;
using System.Windows.Input;

namespace HBLibrary.Wpf.Views;
/// <summary>
/// Interaction logic for HBDarkAccountWindow.xaml
/// </summary>
public partial class HBDarkAccountWindow : Window {
    public HBDarkAccountWindow() {
        InitializeComponent();
    }

    public HBDarkAccountWindow(Window owner, AccountViewModel dataContext) {
        this.Owner = owner;
        InitializeComponent();
        this.DataContext = dataContext;
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
