﻿using System.Windows;

namespace HBLibrary.Wpf.AttachedProperties;
public static class ButtonAttachedProperties
{
    public static readonly DependencyProperty CornerRadiusProperty = DependencyProperty.RegisterAttached(
        "CornerRadius",
        typeof(CornerRadius),
        typeof(ButtonAttachedProperties),
        new FrameworkPropertyMetadata(new CornerRadius(), FrameworkPropertyMetadataOptions.Inherits)
    );

    public static void SetCornerRadius(UIElement element, CornerRadius value)
    {
        element.SetValue(CornerRadiusProperty, value);
    }

    public static CornerRadius GetCornerRadius(UIElement element)
    {
        return (CornerRadius)element.GetValue(CornerRadiusProperty);
    }
}
