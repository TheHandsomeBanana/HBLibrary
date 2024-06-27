using HBLibrary.Wpf.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HBLibrary.Wpf.Services.NavigationService;
public interface INavigationStoreBuilder {
    public INavigationStoreBuilder AddViewModel(ViewModelBase viewModel);
    public Dictionary<Type, ViewModelBase> Build();
}
