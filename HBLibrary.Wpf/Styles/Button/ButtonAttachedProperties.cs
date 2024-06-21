using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace HBLibrary.Wpf.Styles.Buttons;
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
