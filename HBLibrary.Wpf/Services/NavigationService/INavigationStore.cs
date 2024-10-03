using HBLibrary.Wpf.ViewModels;

namespace HBLibrary.Wpf.Services.NavigationService;
public interface INavigationStore {

    public ActiveViewModel this[string parentTypename] { get; set; }
    public void SwitchViewModel(string parentTypename, ViewModelBase viewModel);
    public void SwitchViewModel<TViewModel>(string parentTypename, TViewModel viewModel) where TViewModel : ViewModelBase;
}
