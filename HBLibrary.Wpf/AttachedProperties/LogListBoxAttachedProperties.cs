using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows;
using HBLibrary.Wpf.Controls;
using System.Runtime.CompilerServices;
using System.Windows.Threading;
using System.Windows.Media;

namespace HBLibrary.Wpf.AttachedProperties;

public static class LogListBoxAttachedProperties {
    public static readonly DependencyProperty EnableAutoScrollProperty =
        DependencyProperty.RegisterAttached("EnableAutoScroll", typeof(bool), typeof(LogListBoxAttachedProperties),
            new PropertyMetadata(false, OnEnableAutoScrollChanged));

    private static readonly DependencyPropertyKey IsAutoScrollAttachedPropertyKey =
        DependencyProperty.RegisterAttachedReadOnly("IsAutoScrollAttached", typeof(bool), typeof(LogListBoxAttachedProperties),
            new PropertyMetadata(false));

    public static bool GetEnableAutoScroll(DependencyObject obj) => (bool)obj.GetValue(EnableAutoScrollProperty);
    public static void SetEnableAutoScroll(DependencyObject obj, bool value) => obj.SetValue(EnableAutoScrollProperty, value);
    private static bool GetIsAutoScrollAttached(DependencyObject obj) =>
       (bool)obj.GetValue(IsAutoScrollAttachedPropertyKey.DependencyProperty);

    private static void OnEnableAutoScrollChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) {
        if (d is LogListBox listBox) {
            if ((bool)e.NewValue) {
                // Listen for when the ListBox's ItemsSource is set or changed
                listBox.Loaded += ListBox_Loaded;
                listBox.Unloaded += ListBox_Unloaded;

                AttachCollectionChangedHandler(listBox);
            }
            else {
                DetachCollectionChangedHandler(listBox);
                listBox.Loaded -= ListBox_Loaded;
                listBox.Unloaded -= ListBox_Unloaded;
            }
        }
    }

    private static void ListBox_Loaded(object sender, RoutedEventArgs e) {
        if (sender is LogListBox listBox) {
            // Attach ScrollViewer handlers
            if (FindScrollViewer(listBox) is ScrollViewer scrollViewer) {
                scrollViewer.ScrollChanged += (s, args) => OnScrollChanged(listBox, scrollViewer);
            }

            AttachCollectionChangedHandler(listBox);
        }
    }

    private static void ListBox_Unloaded(object sender, RoutedEventArgs e) {
        if (sender is LogListBox listBox) {
            DetachCollectionChangedHandler(listBox);

            // Detach ScrollViewer handlers
            if (FindScrollViewer(listBox) is ScrollViewer scrollViewer) {
                scrollViewer.ScrollChanged -= (s, args) => OnScrollChanged(listBox, scrollViewer);
            }
        }
    }

    private static void AttachCollectionChangedHandler(LogListBox listBox) {
        if (listBox.ItemsSource is INotifyCollectionChanged collection && !GetIsAutoScrollAttached(listBox)) {
            NotifyCollectionChangedEventHandler handler = (s, args) => {
                if (GetEnableAutoScroll(listBox)) {
                    ScrollToEnd(listBox);
                }
            };

            listBox.SetValue(IsAutoScrollAttachedPropertyKey, true);
            listBox.Tag = handler; // Abuse tag as handler storage
            collection.CollectionChanged += handler;
        }
    }

    private static void DetachCollectionChangedHandler(LogListBox listBox) {
        if (listBox.ItemsSource is INotifyCollectionChanged collection && listBox.Tag is NotifyCollectionChangedEventHandler handler) {
            collection.CollectionChanged -= handler;
            listBox.Tag = null; // Clear the stored handler
            listBox.ClearValue(IsAutoScrollAttachedPropertyKey);
        }
    }

    private static void ScrollToEnd(LogListBox listBox) {
        if (listBox.Items.Count > 0) {
            listBox.Dispatcher.InvokeAsync(() => {
                // Async check -> items could have already been cleared
                if (listBox.Items.Count > 0) {
                    listBox.ScrollIntoView(listBox.Items[^1]);
                }
            }, DispatcherPriority.Background);
        }
    }


    private static ScrollViewer? FindScrollViewer(DependencyObject obj) {
        if (obj is not DependencyObject current) {
            return null;
        }

        for (int i = 0; i < VisualTreeHelper.GetChildrenCount(current); i++) {
            var child = VisualTreeHelper.GetChild(current, i);
            if (child is ScrollViewer sv) {
                return sv;
            }

            ScrollViewer? result = FindScrollViewer(child);
            if (result is not null) {
                return result;
            }
        }

        return null;
    }

    private static void OnScrollChanged(LogListBox listBox, ScrollViewer scrollViewer) {
        // Check if the user scrolled away from the bottom
        bool isAtBottom = Math.Abs(scrollViewer.VerticalOffset - scrollViewer.ScrollableHeight) < 1.0;

        SetEnableAutoScroll(listBox, isAtBottom);
    }
}
