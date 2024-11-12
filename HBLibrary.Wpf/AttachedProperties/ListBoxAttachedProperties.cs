using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows;

namespace HBLibrary.Wpf.AttachedProperties;

public static class ListBoxAttachedProperties {
    public static readonly DependencyProperty EnableAutoScrollProperty = 
        DependencyProperty.RegisterAttached("EnableAutoScroll", typeof(bool), typeof(ListBoxAttachedProperties), new PropertyMetadata(false, OnEnableAutoScrollChanged));

    public static bool GetEnableAutoScroll(DependencyObject obj) {
        return (bool)obj.GetValue(EnableAutoScrollProperty);
    }

    public static void SetEnableAutoScroll(DependencyObject obj, bool value) {
        obj.SetValue(EnableAutoScrollProperty, value);
    }

    private static void OnEnableAutoScrollChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) {
        if (d is ListBox listBox) {
            var items = listBox.ItemsSource as INotifyCollectionChanged;

            if (items != null && (bool)e.NewValue) {
                // Subscribe to the CollectionChanged event
                items.CollectionChanged += (sender, args) => {
                    if (args.Action == NotifyCollectionChangedAction.Add) {
                        // Scroll to the last item when a new item is added
                        listBox.ScrollIntoView(listBox.Items[^1]);
                    }
                };
            }
        }
    }
}
