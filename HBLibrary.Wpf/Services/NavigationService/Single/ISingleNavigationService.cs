using HBLibrary.Wpf.ViewModels;

namespace HBLibrary.Wpf.Services.NavigationService.Single;
public interface ISingleNavigationService<TViewModel> where TViewModel : ViewModelBase {
    public void Navigate();
}