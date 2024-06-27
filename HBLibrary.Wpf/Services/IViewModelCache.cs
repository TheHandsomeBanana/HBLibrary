using HBLibrary.Wpf.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HBLibrary.Wpf.Services;
[Obsolete]
public interface IViewModelCache {
    [Obsolete]
    public void AddOrUpdate<TViewModel>(TViewModel viewModel) where TViewModel : ViewModelBase;
    [Obsolete]
    public void AddOrUpdate(ViewModelBase viewModel);
    [Obsolete]
    public bool TryGet<TViewModel>(out TViewModel? viewModel) where TViewModel : ViewModelBase;
    [Obsolete]
    public bool TryGet(Type type, out ViewModelBase? viewModel);
    [Obsolete]
    public TViewModel GetOrNew<TViewModel>() where TViewModel : ViewModelBase, new();
    [Obsolete]
    public ViewModelBase GetOrNew(Type type);
}
