﻿using System.Windows;

namespace HBLibrary.Wpf.ViewModels;
public class WindowCloser {


    public static bool GetEnableWindowClosing(DependencyObject obj) {
        return (bool)obj.GetValue(EnableWindowClosingProperty);
    }

    public static void SetEnableWindowClosing(DependencyObject obj, bool value) {
        obj.SetValue(EnableWindowClosingProperty, value);
    }

    // Using a DependencyProperty as the backing store for MyProperty.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty EnableWindowClosingProperty =
        DependencyProperty.RegisterAttached("MyProperty", typeof(bool), typeof(WindowCloser), new PropertyMetadata(false, OnEnableWindowClosingChanged));

    private static void OnEnableWindowClosingChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) {
        if (d is Window window) {
            window.Loaded += (s, o) => {
                if (window.DataContext is ICloseableWindow vm) {
                    vm.Close += () => {
                        window.Close();
                    };

                    window.Closing += (a, c) => {
                        c.Cancel = !vm.CanClose();
                    };
                }
            };
        }
    }
}
