using System.Windows;

namespace HBLibrary.Wpf.Styles.TextBox;
public static class PasswordBoxAttachedProperties {
    public static readonly DependencyProperty WatermarkProperty =
        DependencyProperty.RegisterAttached("Watermark", typeof(string), typeof(PasswordBoxAttachedProperties), new PropertyMetadata(string.Empty));

    public static string GetWatermark(DependencyObject obj) {
        return (string)obj.GetValue(WatermarkProperty);
    }

    public static void SetWatermark(DependencyObject obj, string value) {
        obj.SetValue(WatermarkProperty, value);
    }

    public static readonly DependencyProperty IsWatermarkVisibleProperty =
        DependencyProperty.RegisterAttached("IsWatermarkVisible", typeof(bool), typeof(PasswordBoxAttachedProperties), new PropertyMetadata(true));

    public static bool GetIsWatermarkVisible(DependencyObject obj) {
        return (bool)obj.GetValue(IsWatermarkVisibleProperty);
    }

    public static void SetIsWatermarkVisible(DependencyObject obj, bool value) {
        obj.SetValue(IsWatermarkVisibleProperty, value);
    }
}
