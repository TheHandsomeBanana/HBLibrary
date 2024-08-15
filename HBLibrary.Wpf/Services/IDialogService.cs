using HBLibrary.Wpf.ViewModels;
using System.Windows.Controls;

namespace HBLibrary.Wpf.Services;
public interface IDialogService {
    public bool ShowDialog(UserControl dialogContent, ViewModelBase viewModel, string title = "");
    public bool ShowCompactDialog(UserControl dialogContent, ViewModelBase viewModel, string title = "");
}
