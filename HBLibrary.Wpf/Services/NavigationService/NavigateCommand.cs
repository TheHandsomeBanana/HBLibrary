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

    public NavigateCommand(INavigationService navigationService) {
        this.navigationService = navigationService;
    }

    public override void Execute(object? parameter) {
        if (parameter is NavigateCommandParameter commandParameter) {
            navigationService.Navigate(commandParameter.ParentTypename, commandParameter.ViewModelType);
        }
    }
}

public class NavigateCommand<TViewModel> : CommandBase where TViewModel : ViewModelBase {
    private readonly INavigationService navigationService;

    public NavigateCommand(INavigationService navigationService) {
        this.navigationService = navigationService;
    }

    public override void Execute(object? parameter) {
        if(parameter is string parentTypename) {
            navigationService.Navigate<TViewModel>(parentTypename);
        }
    }
}

public class NavigateCommandParameter {
    public Type ViewModelType { get; set; }
    public string ParentTypename { get; set; }

    public NavigateCommandParameter(Type viewModelType, string parentTypename) {
        ViewModelType = viewModelType;
        ParentTypename = parentTypename;
    }
}

