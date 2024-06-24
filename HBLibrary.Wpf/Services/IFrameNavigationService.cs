using HBLibrary.Wpf.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace HBLibrary.Wpf.Services;
public interface IFrameNavigationService {
    void Navigate<TViewModel>(string frameKey, Func<TViewModel> viewModelFactory) where TViewModel : ViewModelBase;
    void SetFrame(string frameKey, Frame frame);
}
