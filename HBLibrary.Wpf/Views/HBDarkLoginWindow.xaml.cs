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
/// Interaction logic for HBDarkLoginWindow.xaml
/// </summary>
public partial class HBDarkLoginWindow : Window {
    public HBDarkLoginWindow() {
        InitializeComponent();
    }
    
    public HBDarkLoginWindow(Window owner) {
        Owner = owner;
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
}