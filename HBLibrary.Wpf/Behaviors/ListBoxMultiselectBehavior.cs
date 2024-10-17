using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls.Primitives;
using System.Windows.Controls;
using System.Windows;
using Microsoft.Xaml.Behaviors;
using System.Collections.ObjectModel;

namespace HBLibrary.Wpf.Behaviors;

public class ListBoxMultiselectBehavior : Behavior<ListBox> {
    public static readonly DependencyProperty SelectedItemsProperty =
             DependencyProperty.Register(
                 nameof(SelectedItems),
                 typeof(ObservableCollection<object>),
                 typeof(ListBoxMultiselectBehavior),
                 new FrameworkPropertyMetadata(new ObservableCollection<object>(), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

    public ObservableCollection<object> SelectedItems {
        get { return (ObservableCollection<object>)GetValue(SelectedItemsProperty); }
        set { SetValue(SelectedItemsProperty, value); }
    }

    protected override void OnAttached() {
        base.OnAttached();

        if (AssociatedObject != null) {
            AssociatedObject.SelectionMode = SelectionMode.Multiple;
            AssociatedObject.SelectionChanged += OnSelectionChanged;
        }
    }

    protected override void OnDetaching() {
        base.OnDetaching();

        if (AssociatedObject != null) {
            AssociatedObject.SelectionChanged -= OnSelectionChanged;
        }
    }

    private void OnSelectionChanged(object sender, SelectionChangedEventArgs e) {
        if (AssociatedObject != null && SelectedItems != null) {
            foreach (object item in e.AddedItems) {
                if (!SelectedItems.Contains(item)) {
                    SelectedItems.Add(item);
                }
            }

            foreach (object item in e.RemovedItems) {
                if (SelectedItems.Contains(item)) {
                    SelectedItems.Remove(item);
                }
            }
        }
    }
}
