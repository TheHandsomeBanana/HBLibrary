using HBLibrary.Wpf.Services.NavigationService.Builder;
using HBLibrary.Wpf.ViewModels;

namespace HBLibrary.Wpf.Services.NavigationService;
public class NavigationStore : INavigationStore {

    private readonly Dictionary<string, ActiveViewModel> activeViewModels = [];

    private readonly NavigationStoreSettings settings;

    public ActiveViewModel this[string parentTypename] {
        get => activeViewModels[parentTypename];
        set => activeViewModels[parentTypename] = value;
    }

    public NavigationStore(NavigationStoreSettings settings, List<string> parentTypeNames) {
        this.settings = settings;
        foreach (string parentTypeName in parentTypeNames) {
            activeViewModels[parentTypeName] = new ActiveViewModel();
        }
    }

    public static INavigationStoreBuilder CreateBuilder() {
        return new NavigationStoreBuilder();
    }

    public void SwitchViewModel(string parentTypename, ViewModelBase viewModel) {
        if (activeViewModels.TryGetValue(parentTypename, out ActiveViewModel? activeViewModel)) {
            if (activeViewModel.ViewModel is null) {
                activeViewModel.ViewModel = viewModel;
                return;
            }

            if (!settings.AllowRenavigation && activeViewModel.ViewModel.GetType() == viewModel.GetType()) {
                return;
            }

            if (settings.DisposeOnLeave && activeViewModel.ViewModel is IDisposable disposableViewModel) {
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
            if (activeViewModel.ViewModel is null) {
                activeViewModel.ViewModel = viewModel;
                return;
            }

            if (!settings.AllowRenavigation && activeViewModel.ViewModel.GetType() == viewModel.GetType()) {
                return;
            }

            if (settings.DisposeOnLeave && activeViewModel.ViewModel is IDisposable disposableViewModel) {
                disposableViewModel.Dispose();
            }

            activeViewModel.ViewModel = viewModel;
        }
        else {
            activeViewModels[parentTypename] = new ActiveViewModel(viewModel);
        }
    }

    public void Clear() {
        foreach (ActiveViewModel viewModel in activeViewModels.Values) {
            if (viewModel.ViewModel is IDisposable disposableViewModel) {
                disposableViewModel.Dispose();

                viewModel.ViewModel = null;
            }
        }
    }

    public void Dispose() {
        foreach (ActiveViewModel? viewModel in activeViewModels.Values) {
            if (viewModel?.ViewModel is IDisposable disposableViewModel) {
                disposableViewModel.Dispose();
            }
        }
    }
}


