using HBLibrary.Wpf.ViewModels;

namespace HBLibrary.Wpf.Services.NavigationService;
public interface INavigationStoreBuilder {
    public INavigationStoreBuilder AddViewModel(ViewModelBase viewModel);
    public Dictionary<Type, ViewModelBase> Build();
}
