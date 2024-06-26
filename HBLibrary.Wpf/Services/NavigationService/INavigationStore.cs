using HBLibrary.Wpf.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HBLibrary.Wpf.Services.NavigationService;
public interface INavigationStore {
    public event Action CurrentViewModelChanged;
    public ViewModelBase CurrentViewModel { get; set; }

}
