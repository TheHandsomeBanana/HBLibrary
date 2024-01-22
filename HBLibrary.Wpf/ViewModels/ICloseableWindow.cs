using System;

namespace HBLibrary.Wpf.ViewModels; 
public interface ICloseableWindow {
    Action Close { get; set; }
    bool CanClose();
}
