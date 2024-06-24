using HBLibrary.Wpf.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HBLibrary.Wpf.Services;
public class ViewModelCache : IViewModelCache {
    private readonly Dictionary<string, ViewModelBase> viewModels = [];

    public void AddOrUpdate<TViewModel>(TViewModel viewModel) where TViewModel : ViewModelBase {
        viewModels[typeof(TViewModel).FullName!] = viewModel;
    }

    public void AddOrUpdate(ViewModelBase viewModel) {
        viewModels[viewModel.GetType().FullName!] = viewModel;
    }

    public TViewModel GetOrNew<TViewModel>() where TViewModel : ViewModelBase, new() {
        if (viewModels.TryGetValue(typeof(TViewModel).FullName!, out ViewModelBase? value))
            return (TViewModel)value;

        return new TViewModel();
    }

    public bool TryGet<TViewModel>(out TViewModel? viewModel) where TViewModel : ViewModelBase {
        bool contains = viewModels.TryGetValue(typeof(TViewModel).FullName!, out ViewModelBase? viewModelBase);
        viewModel = viewModelBase as TViewModel;
        return contains;
    }
}
