using HBLibrary.NetFramework.Wpf.Commands;
using HBLibrary.NetFramework.Wpf.ViewModels;

namespace HBLibrary.NetFramework.Wpf.Navigation {
    public class NavigateCommand<TViewModel> : CommandBase where TViewModel : ViewModelBase {
        private readonly NavigationService<TViewModel> navigationService;

        public NavigateCommand(NavigationService<TViewModel> navigationService) {
            this.navigationService = navigationService;
        }

        public override void Execute(object parameter) {
            navigationService.Navigate();
        }
    }
}
