using HBLibrary.Wpf.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace HBLibrary.Wpf.Services;
public interface IDialogService {
    public bool ShowDialog(UserControl dialogContent, ViewModelBase viewModel, string title = "");
    public bool ShowCompactDialog(UserControl dialogContent, ViewModelBase viewModel, string title = "");
}
