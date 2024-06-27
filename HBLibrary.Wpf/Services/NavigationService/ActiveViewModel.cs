using HBLibrary.Wpf.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HBLibrary.Wpf.Services.NavigationService;
public class ActiveViewModel {
    public event Action? CurrentViewModelChanged;
    private ViewModelBase viewModel;
    public ViewModelBase ViewModel {
        get => viewModel;
        set {
            viewModel = value;
            CurrentViewModelChanged?.Invoke();
        }
    }

    public ActiveViewModel(ViewModelBase viewModel) {
        this.ViewModel = viewModel;
    }
}
