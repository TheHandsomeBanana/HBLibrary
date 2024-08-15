using HBLibrary.Wpf.ViewModels;

namespace HBLibrary.Wpf.Services.NavigationService;
public class NavigationStoreBuilder : INavigationStoreBuilder {
    private readonly Dictionary<Type, ViewModelBase> result = [];
    public INavigationStoreBuilder AddViewModel(ViewModelBase viewModel) {
        result.Add(viewModel.GetType(), viewModel);
        return this;
    }

    public Dictionary<Type, ViewModelBase> Build() {
        return result;
    }
}
