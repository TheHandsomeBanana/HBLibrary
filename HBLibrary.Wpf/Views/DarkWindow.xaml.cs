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
/// Interaction logic for DarkWindow.xaml
/// </summary>
public partial class DarkWindow : Window {
    public DarkWindow() {
        InitializeComponent();
        StateChanged += CustomWindowStateChangeRaised;
    }

    private void CustomWindowStateChangeRaised(object? sender, EventArgs e) {
        if (WindowState == WindowState.Maximized) {
            MainBorder.Margin = new Thickness(8);
            MainBorder.BorderThickness = new Thickness(0);
        }
        else {
            MainBorder.Margin = new Thickness(0);
            MainBorder.BorderThickness = new Thickness(1);
        }
    }

    // Can execute
    private void CommandBinding_CanExecute(object sender, CanExecuteRoutedEventArgs e) {
        e.CanExecute = true;
    }


    // Minimize
    private void CommandBinding_Executed_Minimize(object sender, ExecutedRoutedEventArgs e) {
        SystemCommands.MinimizeWindow(this);
    }

    // Close
    private void CommandBinding_Executed_Close(object sender, ExecutedRoutedEventArgs e) {
        SystemCommands.CloseWindow(this);
    }
}

