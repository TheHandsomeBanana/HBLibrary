using HBLibrary.Wpf.ViewModels;

namespace HBLibrary.Wpf.Services.NavigationService.Single;

public interface ISingleNavigationStore
{
    public event Action? CurrentViewModelChanged;
    public ViewModelBase? CurrentViewModel { get; set; }
}