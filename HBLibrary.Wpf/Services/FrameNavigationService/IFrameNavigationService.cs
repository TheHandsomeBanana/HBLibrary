using HBLibrary.Wpf.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace HBLibrary.Wpf.Services.FrameNavigationService;
public interface IFrameNavigationService {
    void Navigate(string frameKey, Uri pageUri);
    void RegisterFrameNavigation(string frameKey, FrameNavigationModel frame);
    void RegisterFrameNavigation(Func<IFrameNavigationBuilder, Dictionary<string, FrameNavigationModel>> frameNavigationBuilder);
    void RegisterFrame(string frameKey, Frame frame);
    void RegisterFrames(Dictionary<string, Frame> frames);
}
