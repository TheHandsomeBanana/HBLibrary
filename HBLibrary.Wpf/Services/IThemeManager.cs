using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HBLibrary.Wpf.Services;

public interface IThemeManager {
    public event EventHandler? ThemeChanged;
    public void ApplyTheme(string themeName);
    public string GetCurrentTheme();
    IEnumerable<string> GetAvailableThemes();
}
