using HBLibrary.Wpf.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace HBLibrary.Wpf.Services.FrameNavigationService;
public class FrameNavigationModel {
    public string BasePath { get; set; } = "";
    public Frame? Frame { get; set; }
    public Dictionary<Uri, Type> ViewModelMapping { get; }

    public FrameNavigationModel(string basePath, Dictionary<Uri, Type> viewModelMapping) {
        BasePath = basePath;
        ViewModelMapping = viewModelMapping;
    }
}
