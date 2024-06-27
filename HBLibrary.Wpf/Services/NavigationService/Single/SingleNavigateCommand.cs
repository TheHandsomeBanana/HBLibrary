using HBLibrary.Wpf.Commands;
using HBLibrary.Wpf.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HBLibrary.Wpf.Services.NavigationService.Single;
public class SingleNavigateCommand<TViewModel> : CommandBase where TViewModel : ViewModelBase
{
    private readonly ISingleNavigationService<TViewModel> navigationService;
    public SingleNavigateCommand(ISingleNavigationService<TViewModel> navigationService)
    {
        this.navigationService = navigationService;
    }

    public override void Execute(object? parameter)
    {
        navigationService.Navigate();
    }
}

