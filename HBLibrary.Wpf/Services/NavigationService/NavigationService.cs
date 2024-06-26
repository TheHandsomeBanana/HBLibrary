using HBLibrary.Wpf.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HBLibrary.Wpf.Services.NavigationService;
public class NavigationService<TViewModel> : INavigationService<TViewModel> where TViewModel : ViewModelBase {
    private readonly NavigationStore navigationStore;
    private readonly Func<TViewModel> viewModelFactory;

    public NavigationService(NavigationStore navigationStore, Func<TViewModel> viewModelFactory) {
        this.navigationStore = navigationStore;
        this.viewModelFactory = viewModelFactory;
    }

    public void Navigate() {
        navigationStore.CurrentViewModel = viewModelFactory();
    }
}
