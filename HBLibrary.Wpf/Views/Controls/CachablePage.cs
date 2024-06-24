using HBLibrary.Wpf.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace HBLibrary.Wpf.Views.Controls;
public class CachablePage<TViewModel> : Page where TViewModel : ViewModelBase {
    public TViewModel ViewModel { get; }

    protected CachablePage(TViewModel viewModel) {
        ViewModel = viewModel;
        DataContext = viewModel;
    }
}
