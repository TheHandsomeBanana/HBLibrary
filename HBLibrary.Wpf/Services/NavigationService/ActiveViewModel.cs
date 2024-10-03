using HBLibrary.Wpf.ViewModels;

namespace HBLibrary.Wpf.Services.NavigationService;
public class ActiveViewModel {
    public event Action? CurrentViewModelChanged;
    private ViewModelBase? viewModel;
    public ViewModelBase? ViewModel {
        get => viewModel;
        set {
            viewModel = value;
            CurrentViewModelChanged?.Invoke();
        }
    }

    public ActiveViewModel(ViewModelBase viewModel) {
        this.ViewModel = viewModel;
    }

    public ActiveViewModel() {

    }
}
