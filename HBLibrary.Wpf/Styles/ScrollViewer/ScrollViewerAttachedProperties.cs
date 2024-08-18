using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace HBLibrary.Wpf.Styles.ScrollViewer;

// If there is no instantiable class inside a folder, the xaml parser will not recognize it
// as a namespace to use, so this placeholder is required for the
// ScrollViewerAttachedProperties to be recognized 
public class placeholder {

}

public static class ScrollViewerAttachedProperties {
    public static bool GetScrollOnMouseOver(DependencyObject obj) {
        return (bool)obj.GetValue(ScrollOnMouseOverProperty);
    }

    public static void SetScrollOnMouseOver(DependencyObject obj, bool value) {
        obj.SetValue(ScrollOnMouseOverProperty, value);
    }

    public static readonly DependencyProperty ScrollOnMouseOverProperty =
        DependencyProperty.RegisterAttached("ScrollOnMouseOver", typeof(bool),
            typeof(ScrollViewerAttachedProperties), new PropertyMetadata(false, OnScrollOnMouseOverChanged));

    private static void OnScrollOnMouseOverChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) {
        if (d is System.Windows.Controls.ScrollViewer scrollViewer) {
            if ((bool)e.NewValue) {
                scrollViewer.PreviewMouseWheel += ScrollViewer_PreviewMouseWheel;
            }
            else {
                scrollViewer.PreviewMouseWheel -= ScrollViewer_PreviewMouseWheel;
            }
        }
    }

    private static void ScrollViewer_PreviewMouseWheel(object sender, MouseWheelEventArgs e) {
        System.Windows.Controls.ScrollViewer? scrollViewer = sender as System.Windows.Controls.ScrollViewer;

        if (scrollViewer is not null) {
            if (Keyboard.Modifiers == ModifierKeys.Shift) {
                scrollViewer.ScrollToHorizontalOffset(scrollViewer.HorizontalOffset - (e.Delta / 2));
            }
            else {
                scrollViewer.ScrollToVerticalOffset(scrollViewer.VerticalOffset - (e.Delta / 2));
            }

            e.Handled = true;
        }
    }
}