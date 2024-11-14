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
            AttachCollectionChangedHandler(listBox);
        }
    }

    private static void ListBox_Unloaded(object sender, RoutedEventArgs e) {
        if (sender is LogListBox listBox) {
            DetachCollectionChangedHandler(listBox);
        }
    }

    private static void AttachCollectionChangedHandler(LogListBox listBox) {
        if (listBox.ItemsSource is INotifyCollectionChanged collection && !GetIsAutoScrollAttached(listBox)) {
            collection.CollectionChanged += (s, args) => ScrollToEnd(listBox);
            listBox.SetValue(IsAutoScrollAttachedPropertyKey, true);
        }
    }

    private static void DetachCollectionChangedHandler(LogListBox listBox) {
        if (listBox.ItemsSource is INotifyCollectionChanged collection) {
            collection.CollectionChanged -= (s, args) => ScrollToEnd(listBox);
            listBox.ClearValue(IsAutoScrollAttachedPropertyKey);
        }
    }

    private static void ScrollToEnd(LogListBox listBox) {
        if (listBox.Items.Count > 0) {
            listBox.Dispatcher.InvokeAsync(() => {
                listBox.ScrollIntoView(listBox.Items[^1]);
            }, DispatcherPriority.Background);
        }
    }

    private static bool GetIsAutoScrollAttached(DependencyObject obj) =>
        (bool)obj.GetValue(IsAutoScrollAttachedPropertyKey.DependencyProperty);
}
