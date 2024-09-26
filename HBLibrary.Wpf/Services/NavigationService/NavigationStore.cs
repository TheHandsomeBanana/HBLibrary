using HBLibrary.Wpf.ViewModels;

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
            if (activeViewModel.ViewModel is IDisposable disposableViewModel) {
                disposableViewModel.Dispose();
            }

            activeViewModel.ViewModel = viewModel;
        }
        else {
            activeViewModels[parentTypename] = new ActiveViewModel(viewModel);
        }
    }

    public void SwitchViewModel<TViewModel>(string parentTypename, TViewModel viewModel) where TViewModel : ViewModelBase {
        if (activeViewModels.TryGetValue(parentTypename, out ActiveViewModel? activeViewModel)) {
            if(activeViewModel.ViewModel is IDisposable disposableViewModel) {
                disposableViewModel.Dispose();
            }

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


