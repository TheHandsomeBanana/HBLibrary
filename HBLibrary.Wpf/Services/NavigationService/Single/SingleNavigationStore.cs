using HBLibrary.Wpf.ViewModels;

namespace HBLibrary.Wpf.Services.NavigationService.Single;
public class SingleNavigationStore : ISingleNavigationStore {
    public event Action? CurrentViewModelChanged;

    private ViewModelBase? currentViewModel;
    public ViewModelBase? CurrentViewModel {
        get => currentViewModel;
        set {
            currentViewModel = value;
            CurrentViewModelChanged?.Invoke();
        }
    }

}
