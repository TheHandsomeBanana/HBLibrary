using HBLibrary.Wpf.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HBLibrary.Wpf.Services;
public interface IViewModelCache {
    public void AddOrUpdate<TViewModel>(TViewModel viewModel) where TViewModel : ViewModelBase;
    public void AddOrUpdate(ViewModelBase viewModel);
    public bool TryGet<TViewModel>(out TViewModel? viewModel) where TViewModel : ViewModelBase;
    public TViewModel GetOrNew<TViewModel>() where TViewModel : ViewModelBase, new();
}
