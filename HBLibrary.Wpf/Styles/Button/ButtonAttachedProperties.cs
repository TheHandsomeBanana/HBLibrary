using System.Windows;

namespace HBLibrary.Wpf.Styles.Button;
public static class ButtonAttachedProperties {
    public static readonly DependencyProperty CornerRadiousProperty = DependencyProperty.RegisterAttached(
        "CornerRadius",
        typeof(CornerRadius),
        typeof(ButtonAttachedProperties),
        new FrameworkPropertyMetadata(new CornerRadius(), FrameworkPropertyMetadataOptions.Inherits)
    );

    public static void SetCornerRadius(UIElement element, CornerRadius value) {
        element.SetValue(CornerRadiousProperty, value);
    }

    public static CornerRadius GetCornerRadius(UIElement element) {
        return (CornerRadius)element.GetValue(CornerRadiousProperty);
    }
}
