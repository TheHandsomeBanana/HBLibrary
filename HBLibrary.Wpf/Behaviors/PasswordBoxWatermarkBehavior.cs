using HBLibrary.Wpf.AttachedProperties;
using Microsoft.Xaml.Behaviors;
using System.Windows;
using System.Windows.Controls;

namespace HBLibrary.Wpf.Behaviors;

public class PasswordBoxWatermarkBehavior : Behavior<PasswordBox> {
    protected override void OnAttached() {
        base.OnAttached();
        AssociatedObject.PasswordChanged += OnPasswordChanged;
        UpdateWatermarkVisibility();
    }

    protected override void OnDetaching() {
        base.OnDetaching();
        AssociatedObject.PasswordChanged -= OnPasswordChanged;
    }

    private void OnPasswordChanged(object sender, RoutedEventArgs e) {
        UpdateWatermarkVisibility();
    }

    private void UpdateWatermarkVisibility() {
        PasswordBox passwordBox = AssociatedObject;
        bool isWatermarkVisible = string.IsNullOrEmpty(passwordBox.Password);
        PasswordBoxAttachedProperties.SetIsWatermarkVisible(passwordBox, isWatermarkVisible);
    }
}
