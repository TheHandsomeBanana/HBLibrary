using HBLibrary.Wpf.ViewModels;

namespace HBLibrary.Wpf.Services.NavigationService;
public class NavigationService : INavigationService {
    private readonly INavigationStore navigationStore;

    public NavigationService(INavigationStore navigationStore) {
        this.navigationStore = navigationStore;
    }

    public void Navigate<TViewModel>(string parentTypename, TViewModel viewModel) where TViewModel : ViewModelBase {
        navigationStore.SwitchViewModel(parentTypename, viewModel);
    }

    public void Navigate(string parentTypename, ViewModelBase viewModelBase) {
        navigationStore.SwitchViewModel(parentTypename, viewModelBase);
    }
}
