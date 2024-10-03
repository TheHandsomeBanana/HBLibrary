using HBLibrary.Wpf.ViewModels;

namespace HBLibrary.Wpf.Services.NavigationService.Builder;

public interface INavigationStoreBuilder {

    public INavigationStoreBuilder DisposeOnLeave();
    public INavigationStoreBuilder AllowRenavigation();
    public INavigationStoreBuilder AddParentTypename(string typeName);
    public INavigationStore Build();
}