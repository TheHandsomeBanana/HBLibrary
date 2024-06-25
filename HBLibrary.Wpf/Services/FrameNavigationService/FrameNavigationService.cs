using HBLibrary.Wpf.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Navigation;

namespace HBLibrary.Wpf.Services.FrameNavigationService;
public class FrameNavigationService : IFrameNavigationService {
    private readonly IViewModelCache viewModelCache;
    private readonly Dictionary<string, FrameNavigationModel> frames = [];

    public FrameNavigationService(IViewModelCache viewModelCache) {
        this.viewModelCache = viewModelCache;
    }

    public void Navigate(string frameKey, Uri pageUri) {
        if (!frames.TryGetValue(frameKey, out FrameNavigationModel? frameModel))
            throw new InvalidOperationException($"Frame model with key {frameKey} not found.");

        if (frameModel.Frame == null) {
            throw new InvalidOperationException($"Frame with key {frameKey} not registered.");
        }


        if (frameModel.Frame.CurrentSource is not null) {

            // Frame will not have absolut path as CurrentSource
            // -> Combine with BasePath
            Uri currentSource = new Uri(frameModel.BasePath + frameModel.Frame.CurrentSource.ToString());

            if (!frameModel.ViewModelMapping.TryGetValue(currentSource, out Type? viewModelType)) {
                throw new InvalidOperationException($"Page Uri {frameModel.Frame.CurrentSource} not found.");
            }

            if (viewModelCache.TryGet(viewModelType, out ViewModelBase? viewModel)) {
                viewModelCache.AddOrUpdate((ViewModelBase)((Page)frameModel.Frame.Content).DataContext);
            }
        }

        if (frameModel.ViewModelMapping.TryGetValue(pageUri, out Type? newViewModelType)) {
            ViewModelBase newViewModel = viewModelCache.GetOrNew(newViewModelType);
            frameModel.Frame.Navigate(pageUri, newViewModel);
        }
        else {
            frameModel.Frame.Navigate(pageUri);
        }
    }

    public void RegisterFrame(string frameKey, Frame frame) {
        if (!frames.TryGetValue(frameKey, out FrameNavigationModel? frameNavigationModel)) {
            throw new InvalidOperationException($"Frame with key {frameKey} not found.");
        }

        if (frameNavigationModel.Frame != null) {
            return;
        }

        frameNavigationModel.Frame = frame;
        frame.Navigated += OnFrameNavigated;
    }

    public void RegisterFrameNavigation(string frameKey, FrameNavigationModel frame) {
        if (frames.ContainsKey(frameKey))
            throw new InvalidOperationException($"Frame with key {frameKey} is already set.");

        frames[frameKey] = frame;
    }

    public void RegisterFrameNavigation(Func<IFrameNavigationBuilder, Dictionary<string, FrameNavigationModel>> frameNavigationBuilder) {
        Dictionary<string, FrameNavigationModel> frames = frameNavigationBuilder(new FrameNavigationBuilder());
        foreach (KeyValuePair<string, FrameNavigationModel> frame in frames) {
            RegisterFrameNavigation(frame.Key, frame.Value);
        }
    }

    public void RegisterFrames(Dictionary<string, Frame> frames) {
        foreach (KeyValuePair<string, Frame> frame in frames) {
            RegisterFrame(frame.Key, frame.Value);
        }
    }

    private void OnFrameNavigated(object sender, NavigationEventArgs e) {
        Page page = (Page)e.Content;

        if (e.ExtraData is ViewModelBase viewModelBase) {
            page.DataContext = viewModelBase;
        }
    }
}
