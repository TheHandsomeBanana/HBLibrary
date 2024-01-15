
using HBLibrary.NetFramework.Wpf.ViewModels;

namespace HBLibrary.NetFramework.Wpf.Navigation {
    public class NavigationStore {
        private ViewModelBase currentViewModel;
        public ViewModelBase CurrentViewModel {
            get => currentViewModel;
            set {
                currentViewModel = value;
            }
        }
    }
}
