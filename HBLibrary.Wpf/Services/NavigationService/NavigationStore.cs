using HBLibrary.Wpf.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HBLibrary.Wpf.Services.NavigationService;
public class NavigationStore : INavigationStore {

    private readonly Dictionary<string, ActiveViewModel> activeViewModels = [];


    public ActiveViewModel this[string parentTypename] {
        get => activeViewModels[parentTypename];
        set => activeViewModels[parentTypename] = value;
    }

    public NavigationStore() {
    }    

    public void SwitchViewModel(string parentTypename, ViewModelBase viewModel) {
        if (activeViewModels.TryGetValue(parentTypename, out ActiveViewModel? activeViewModel)) {
            activeViewModel.ViewModel = viewModel;
        }
        else {
            activeViewModels[parentTypename] = new ActiveViewModel(viewModel);
        }
    }

    public void SwitchViewModel<TViewModel>(string parentTypename, TViewModel viewModel) where TViewModel : ViewModelBase {
        if (activeViewModels.TryGetValue(parentTypename, out ActiveViewModel? activeViewModel)) {
            activeViewModel.ViewModel = viewModel;
        }
        else {
            activeViewModels[parentTypename] = new ActiveViewModel(viewModel);
        }
    }

    public void AddDefaultViewModel(string parentTypename, ViewModelBase viewModel) {
        activeViewModels[parentTypename] = new ActiveViewModel(viewModel);
    }
}


