using HBLibrary.Wpf.Commands;
using HBLibrary.Wpf.ViewModels;

namespace HBLibrary.Wpf.Navigation; 
public class NavigateCommand<TViewModel> : CommandBase where TViewModel : ViewModelBase {
    private readonly NavigationService<TViewModel> navigationService;

    public NavigateCommand(NavigationService<TViewModel> navigationService) {
        this.navigationService = navigationService;
    }

    public override void Execute(object parameter) {
        navigationService.Navigate();
    }
}
