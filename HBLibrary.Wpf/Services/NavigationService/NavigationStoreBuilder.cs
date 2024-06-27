using HBLibrary.Wpf.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
