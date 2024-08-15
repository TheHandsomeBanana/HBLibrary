using HBLibrary.Wpf.ViewModels;

namespace HBLibrary.Wpf.Services.NavigationService;
public interface INavigationService {
    public void Navigate<TViewModel>(string parentTypename, TViewModel viewModel) where TViewModel : ViewModelBase;
    public void Navigate(string parentTypename, ViewModelBase viewModel);
}
