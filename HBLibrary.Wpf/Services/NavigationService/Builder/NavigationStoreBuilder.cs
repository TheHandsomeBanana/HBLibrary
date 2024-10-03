using HBLibrary.Wpf.ViewModels;
using Microsoft.Graph.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HBLibrary.Wpf.Services.NavigationService.Builder;
internal class NavigationStoreBuilder : INavigationStoreBuilder {
    private NavigationStoreSettings settings = new NavigationStoreSettings();
    private readonly List<string> parentTypeNames = [];
    public INavigationStoreBuilder AddParentTypename(string typeName) {
        parentTypeNames.Add(typeName);
        return this;
    }

    public INavigationStore Build() {
        return new NavigationStore(settings, parentTypeNames);
    }

    public INavigationStoreBuilder DisposeOnLeave() {
        settings.DisposeOnLeave = true;
        return this;
    }

    /// <summary>
    /// Allows renavigating to the same ViewModel type
    /// </summary>
    /// <returns></returns>
    public INavigationStoreBuilder AllowRenavigation() {
        settings.AllowRenavigation = true;
        return this;
    }
}
