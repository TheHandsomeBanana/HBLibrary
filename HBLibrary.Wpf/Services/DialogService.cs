﻿using HBLibrary.Wpf.ViewModels;
using HBLibrary.Wpf.Views;
using System.Windows.Controls;

namespace HBLibrary.Wpf.Services;
public class DialogService : IDialogService {


    public bool ShowDialog(UserControl dialogContent, ViewModelBase viewModel, string title = "") {
        DarkWindow window = new DarkWindow {
            DataContext = viewModel
        };

        window.ContentControl.Content = dialogContent;
        window.txbTitle.Text = title;
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

        window.txbTitle.Text = title;
        return window.ShowDialog() == true;
    }
}
