using HBLibrary.Wpf.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HBLibrary.Wpf.Services.NavigationService;
public interface INavigationService {
    public void Navigate<TViewModel>(string parentTypename, TViewModel viewModel) where TViewModel : ViewModelBase;
    public void Navigate(string parentTypename, ViewModelBase viewModel);
}
