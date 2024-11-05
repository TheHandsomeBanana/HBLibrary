using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace HBLibrary.Wpf.Services;

public static class StaticThemeManager {
    private static readonly Uri DarkThemeUri = new Uri("pack://application:,,,/HBLibrary.Wpf;component/Themes/HBDark.xaml");
    private static readonly Uri LightThemeUri = new Uri("pack://application:,,,/HBLibrary.Wpf;component/Themes/HBLight.xaml");

    public static void ApplyTheme(StaticThemes theme) {
        Application.Current.Resources.MergedDictionaries.Clear();

        Uri themeUri;
        switch (theme) {
            case StaticThemes.Dark:
                themeUri = DarkThemeUri;
                break;
            case StaticThemes.Light:
                themeUri = LightThemeUri;
                break;
            default:
                throw new NotSupportedException($"{theme} is not supported.");
        }

        ResourceDictionary themeDictionary = new ResourceDictionary { Source = themeUri };
        Application.Current.Resources.MergedDictionaries.Add(themeDictionary);
    }
}

public enum StaticThemes {
    Light,
    Dark
}
