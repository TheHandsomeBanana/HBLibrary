using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace HBLibrary.Wpf.Services;

public class ThemeManager : IThemeManager {
    private readonly string themeDirectory;
    private readonly Dictionary<string, Uri> themes = [];
    private string? currentTheme;

    public event EventHandler? ThemeChanged;

    public ThemeManager(string themeDirectory) {
        this.themeDirectory = themeDirectory;
        LoadThemes();
    }

    private void LoadThemes() {
        themes.Clear();

        foreach (var filePath in Directory.GetFiles(themeDirectory, "*.xaml")) {
            var themeName = Path.GetFileNameWithoutExtension(filePath);
            themes[themeName] = new Uri(filePath, UriKind.Absolute);
        }
    }

    public void ApplyTheme(string themeName) {
        if (!themes.TryGetValue(themeName, out Uri? themeUri)) {
            throw new ArgumentException($"Theme '{themeName}' not found.");
        }

        // Clear current resources and load the selected theme
        Application.Current.Resources.MergedDictionaries.Clear();
        Application.Current.Resources.MergedDictionaries.Add(new ResourceDictionary { Source = themeUri });

        // Update current theme and raise ThemeChanged event
        currentTheme = themeName;
        ThemeChanged?.Invoke(this, EventArgs.Empty);
    }

    public IEnumerable<string> GetAvailableThemes() => themes.Keys;
    public string GetCurrentTheme() => currentTheme ?? "No theme applied";

}
