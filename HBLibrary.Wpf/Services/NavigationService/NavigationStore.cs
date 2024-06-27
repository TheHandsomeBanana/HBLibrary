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
    private Dictionary<Type, ViewModelBase> viewModelStore = [];

    public ActiveViewModel this[string parentTypename] {
        get => activeViewModels[parentTypename];
        set => activeViewModels[parentTypename] = value;
    }

    public NavigationStore() { }
    public NavigationStore(Func<INavigationStoreBuilder, Dictionary<Type, ViewModelBase>> builder) {
        InitViewModelInstances(builder);
    }

    public void InitViewModelInstances(Func<INavigationStoreBuilder, Dictionary<Type, ViewModelBase>> builder) {
        viewModelStore = builder(new NavigationStoreBuilder());
    }

    public void SwitchViewModel(string parentTypename, Type viewModelType) {
        if (!viewModelStore.TryGetValue(viewModelType, out ViewModelBase? viewModel)) {
            throw new InvalidOperationException($"ViewModel of type {viewModelType} is not registered.");
        }

        if (activeViewModels.TryGetValue(parentTypename, out ActiveViewModel? activeViewModel)) {
            activeViewModel.ViewModel = viewModel;
        }
        else {
            activeViewModels[parentTypename] = new ActiveViewModel(viewModel);
        }
    }

    public void SwitchViewModel<TViewModel>(string parentTypename) {
        SwitchViewModel(parentTypename, typeof(TViewModel));
    }

    public TViewModel GetStoredViewModel<TViewModel>() where TViewModel : ViewModelBase {
        return (TViewModel)viewModelStore[typeof(TViewModel)];
    }
}


