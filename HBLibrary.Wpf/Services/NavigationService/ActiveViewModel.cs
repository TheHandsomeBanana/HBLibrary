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

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    public ActiveViewModel(ViewModelBase viewModel) {
        this.ViewModel = viewModel;
    }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
}
