using HBLibrary.Wpf.Commands;
using HBLibrary.Wpf.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HBLibrary.Wpf.Services.NavigationService;
public class NavigateCommand : CommandBase {
    private readonly INavigationService navigationService;
    private readonly Func<ViewModelBase> createViewModel;

    public NavigateCommand(INavigationService navigationService, Func<ViewModelBase> createViewModel) {
        this.navigationService = navigationService;
        this.createViewModel = createViewModel;
    }

    public override void Execute(object? parameter) {
        if (parameter is string parentTypename) {
            navigationService.Navigate(parentTypename, createViewModel());
        }
    }
}

public class NavigateCommand<TViewModel> : CommandBase where TViewModel : ViewModelBase {
    private readonly INavigationService navigationService;
    private readonly Func<TViewModel> createViewModel;

    public NavigateCommand(INavigationService navigationService, Func<TViewModel> createViewModel) {
        this.navigationService = navigationService;
        this.createViewModel = createViewModel;
    }

    public override void Execute(object? parameter) {
        if(parameter is string parentTypename) {
            navigationService.Navigate<TViewModel>(parentTypename, createViewModel());
        }
    }
}
