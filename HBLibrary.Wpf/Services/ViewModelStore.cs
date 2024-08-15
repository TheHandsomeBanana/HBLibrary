using HBLibrary.Wpf.Services.NavigationService;
using HBLibrary.Wpf.ViewModels;

namespace HBLibrary.Wpf.Services;
public class ViewModelStore : IViewModelStore {
    private Dictionary<Type, ViewModelBase> viewModelStore = [];

    public ViewModelStore() { }

    public ViewModelStore(Func<INavigationStoreBuilder, Dictionary<Type, ViewModelBase>> builder) {
        InitViewModelInstances(builder);
    }

    public TViewModel GetStoredViewModel<TViewModel>() where TViewModel : ViewModelBase {
        return (TViewModel)viewModelStore[typeof(TViewModel)];
    }

    public void InitViewModelInstances(Func<INavigationStoreBuilder, Dictionary<Type, ViewModelBase>> builder) {
        viewModelStore = builder(new NavigationStoreBuilder());
    }

    public bool TryGetValue<TViewModel>(out TViewModel? viewModel) where TViewModel : ViewModelBase {
        bool contains = TryGetValue(typeof(TViewModel), out ViewModelBase? viewModelBase);
        viewModel = viewModelBase as TViewModel;
        return contains;
    }

    public bool TryGetValue(Type viewModelType, out ViewModelBase? viewModel) {
        return viewModelStore.TryGetValue(viewModelType, out viewModel);
    }
}
