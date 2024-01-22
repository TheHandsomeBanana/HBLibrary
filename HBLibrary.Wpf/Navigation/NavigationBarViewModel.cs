using HBLibrary.NetFramework.Wpf.ViewModels;
using System.Windows.Input;

namespace HBLibrary.NetFramework.Wpf.Navigation {
    public class NavigationBarViewModel : ViewModelBase {
        public ICommand[] NavigationCommands { get; }
        public NavigationBarViewModel(params ICommand[] navigationCommands) {
            this.NavigationCommands = navigationCommands;
        }
    }
}
