
using HBLibrary.Wpf.ViewModels;

namespace HBLibrary.Wpf.Navigation; 
public class NavigationStore {
    private ViewModelBase currentViewModel;
    public ViewModelBase CurrentViewModel {
        get => currentViewModel;
        set {
            currentViewModel = value;
        }
    }
}
