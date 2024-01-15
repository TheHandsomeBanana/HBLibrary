using HBLibrary.NetFramework.Wpf.ViewModels;
using System;

namespace HBLibrary.NetFramework.Wpf.Navigation {
    public class NavigationService<TViewModel> where TViewModel : ViewModelBase {
        private readonly NavigationStore navigationStore;
        private readonly Func<TViewModel> createViewModel;

        public NavigationService(NavigationStore navigationStore, Func<TViewModel> createViewModel) {
            this.navigationStore = navigationStore;
            this.createViewModel = createViewModel;
        }

        public void Navigate() {
            navigationStore.CurrentViewModel = createViewModel();
        }
    }
}
