using HBLibrary.Wpf.Commands;
using HBLibrary.Wpf.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HBLibrary.Wpf.Services.NavigationService;
public class NavigateCommand<TViewModel> : CommandBase where TViewModel : ViewModelBase {
    private readonly INavigationService<TViewModel> navigationService;
    public NavigateCommand(INavigationService<TViewModel> navigationService) {
        this.navigationService = navigationService;
    }

    public override void Execute(object? parameter) {
        navigationService.Navigate();
    }
}
