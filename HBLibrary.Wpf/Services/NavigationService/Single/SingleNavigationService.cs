using HBLibrary.Wpf.ViewModels;

namespace HBLibrary.Wpf.Services.NavigationService.Single;
public class SingleNavigationService<TViewModel> : ISingleNavigationService<TViewModel> where TViewModel : ViewModelBase {
    private readonly ISingleNavigationStore navigationStore;
    private readonly Func<TViewModel> viewModelFactory;

    public SingleNavigationService(ISingleNavigationStore navigationStore, Func<TViewModel> viewModelFactory) {
        this.navigationStore = navigationStore;
        this.viewModelFactory = viewModelFactory;
    }

    public void Navigate() {
        navigationStore.CurrentViewModel = viewModelFactory();
    }
}


