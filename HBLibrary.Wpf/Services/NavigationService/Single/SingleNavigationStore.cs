using HBLibrary.Wpf.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HBLibrary.Wpf.Services.NavigationService.Single;
public class SingleNavigationStore : ISingleNavigationStore
{
    public event Action? CurrentViewModelChanged;

    private ViewModelBase currentViewModel;
    public ViewModelBase CurrentViewModel
    {
        get => currentViewModel;
        set
        {
            currentViewModel = value;
            CurrentViewModelChanged?.Invoke();
        }
    }

}
