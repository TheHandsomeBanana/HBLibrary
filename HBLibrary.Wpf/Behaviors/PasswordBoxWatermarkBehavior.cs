using HBLibrary.Wpf.Styles.TextBox;
using Microsoft.Xaml.Behaviors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
