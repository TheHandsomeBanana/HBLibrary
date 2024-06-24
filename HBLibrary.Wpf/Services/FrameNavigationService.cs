using HBLibrary.Wpf.ViewModels;
using HBLibrary.Wpf.Views.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace HBLibrary.Wpf.Services;
public class FrameNavigationService : IFrameNavigationService {
    private readonly IViewModelCache viewModelCache;
    private readonly Dictionary<string, Frame> frames = [];
    private readonly Dictionary<string, Page> currentPages = [];

    public FrameNavigationService(IViewModelCache viewModelCache) {
        this.viewModelCache = viewModelCache;
    }

    public void Navigate<TViewModel>(string frameKey, Func<TViewModel> viewModelFactory) where TViewModel : ViewModelBase {
        if (!frames.TryGetValue(frameKey, out Frame? frame))
            throw new InvalidOperationException($"Frame with key {frameKey} not found.");

        if (currentPages.TryGetValue(frameKey, out Page? currentPage) && currentPage is CachablePage<ViewModelBase> cachablePage) {
            cachablePage.ViewModel.SaveState(viewModelCache);
        }

        var viewModel = viewModelFactory();
        var pageType = typeof(CachablePage<TViewModel>);
        var page = Activator.CreateInstance(pageType, viewModel) as Page;

        frame.Navigate(page);
    }

    public void SetFrame(string frameKey, Frame frame) {
        if (frames.ContainsKey(frameKey))
            throw new InvalidOperationException($"Frame with key {frameKey} is already set.");

        frames[frameKey] = frame;
        frame.Navigated += (_, e) => OnFrameNavigated(frameKey, e);
    }

    private void OnFrameNavigated(string frameKey, System.Windows.Navigation.NavigationEventArgs e) {
        currentPages[frameKey] = (Page)e.Content;
    }
}
