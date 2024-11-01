using System.Windows.Media;

namespace HBLibrary.Wpf.Extensions;
public static class BrushHelper {
    public static SolidColorBrush GetColorFromHex(string value) {
        if (!value.StartsWith('#'))
            throw new ArgumentException("Value must start with '#'", nameof(value));

        return (SolidColorBrush)new BrushConverter().ConvertFrom(value)!;
    }
}
