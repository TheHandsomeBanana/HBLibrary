using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
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



    public static readonly DependencyProperty VerticalScrollBarMarginProperty =
             DependencyProperty.RegisterAttached(
                 "VerticalScrollBarMargin",
                 typeof(Thickness),
                 typeof(ScrollViewerAttachedProperties),
                 new PropertyMetadata(new Thickness(0), OnVerticalScrollBarMarginChanged));

    public static readonly DependencyProperty HorizontalScrollBarMarginProperty =
        DependencyProperty.RegisterAttached(
            "HorizontalScrollBarMargin",
            typeof(Thickness),
            typeof(ScrollViewerAttachedProperties),
            new PropertyMetadata(new Thickness(0), OnHorizontalScrollBarMarginChanged));

    public static void SetVerticalScrollBarMargin(UIElement element, Thickness value) {
        element.SetValue(VerticalScrollBarMarginProperty, value);
    }

    public static Thickness GetVerticalScrollBarMargin(UIElement element) {
        return (Thickness)element.GetValue(VerticalScrollBarMarginProperty);
    }

    public static void SetHorizontalScrollBarMargin(UIElement element, Thickness value) {
        element.SetValue(HorizontalScrollBarMarginProperty, value);
    }

    public static Thickness GetHorizontalScrollBarMargin(UIElement element) {
        return (Thickness)element.GetValue(HorizontalScrollBarMarginProperty);
    }

    private static void OnVerticalScrollBarMarginChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) {
        if (d is System.Windows.Controls.ScrollViewer scrollViewer) {
            ApplyMarginToScrollBar(scrollViewer, "PART_VerticalScrollBar", (Thickness)e.NewValue);
        }
    }

    private static void OnHorizontalScrollBarMarginChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) {
        if (d is System.Windows.Controls.ScrollViewer scrollViewer) {
            ApplyMarginToScrollBar(scrollViewer, "PART_HorizontalScrollBar", (Thickness)e.NewValue);
        }
    }

    private static void ApplyMarginToScrollBar(System.Windows.Controls.ScrollViewer scrollViewer, string partName, Thickness margin) {
        if (scrollViewer.Template != null) {
            scrollViewer.ApplyTemplate();
            var scrollBar = scrollViewer.Template.FindName(partName, scrollViewer) as ScrollBar;
            if (scrollBar != null) {
                scrollBar.Margin = margin;
            }
        }
    }
}