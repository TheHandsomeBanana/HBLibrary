using HBLibrary.Wpf.Services.NavigationService;
using HBLibrary.Wpf.ViewModels;

namespace HBLibrary.Wpf.Services;
public interface IViewModelStore {
    public void InitViewModelInstances(Func<INavigationStoreBuilder, Dictionary<Type, ViewModelBase>> builder);
    public TViewModel GetStoredViewModel<TViewModel>() where TViewModel : ViewModelBase;
    public bool TryGetValue<TViewModel>(out TViewModel? viewModel) where TViewModel : ViewModelBase;
    public bool TryGetValue(Type viewModelType, out ViewModelBase? viewModel);

}
