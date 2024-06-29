using HBLibrary.Wpf.ViewModels;
using HBLibrary.Wpf.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Shell;

namespace HBLibrary.Wpf.Services;
public class DialogService : IDialogService {


    public bool ShowDialog(UserControl dialogContent, ViewModelBase viewModel, string title = "") {
        DarkWindow window = new DarkWindow {
            DataContext = viewModel
        };

        window.ContentControl.Content = dialogContent;
        window.Title.Text = title;
        return window.ShowDialog() == true;
    }

    public bool ShowCompactDialog(UserControl dialogContent, ViewModelBase viewModel, string title = "") {
        CompactWindow window = new CompactWindow {
            DataContext = viewModel,
            MinHeight = dialogContent.MinHeight + 32, // WindowChrome Height + Border
            MinWidth = dialogContent.MinWidth + 2 // Border
        };

        window.ContentControl.Content = dialogContent;

        if (dialogContent.MinHeight > 0) {
            window.ContentControl.MinHeight = dialogContent.MinHeight;
        }

        if (dialogContent.MinWidth > 0) {
            window.ContentControl.MinWidth = dialogContent.MinWidth;
        }

        window.Title.Text = title;
        return window.ShowDialog() == true;
    }
}
