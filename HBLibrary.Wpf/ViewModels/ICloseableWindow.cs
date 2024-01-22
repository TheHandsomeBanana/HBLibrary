using System;

namespace HBLibrary.NetFramework.Wpf.ViewModels {
    public interface ICloseableWindow {
        Action Close { get; set; }
        bool CanClose();
    }
}
