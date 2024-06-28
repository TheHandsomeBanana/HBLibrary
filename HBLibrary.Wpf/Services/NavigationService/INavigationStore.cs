using HBLibrary.Wpf.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HBLibrary.Wpf.Services.NavigationService;
public interface INavigationStore {

    public ActiveViewModel this[string parentTypename] { get; set; }
    public void SwitchViewModel(string parentTypename, Type viewModelType);
    public void SwitchViewModel<TViewModel>(string parentTypename);
}
