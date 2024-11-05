using System.Windows;
using System.Windows.Shapes;

namespace HBLibrary.Wpf.AttachedProperties;
public static class MenuItemAttachedProperties
{
    public static readonly DependencyProperty IconPathProperty =
            DependencyProperty.RegisterAttached("IconPath", typeof(Path), typeof(MenuItemAttachedProperties), new PropertyMetadata(null));

    public static Path GetIconPath(DependencyObject obj)
    {
        return (Path)obj.GetValue(IconPathProperty);
    }

    public static void SetIconPath(DependencyObject obj, Path value)
    {
        obj.SetValue(IconPathProperty, value);
    }
}
