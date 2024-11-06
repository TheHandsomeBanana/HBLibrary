using Microsoft.Xaml.Behaviors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace HBLibrary.Wpf.Behaviors;
public class ListBoxDragDropBehavior : Behavior<ListBox> {
    private Point dragStartPoint;




    public Type DragDropDataType {
        get { return (Type)GetValue(DragDropDataTypeProperty); }
        set { SetValue(DragDropDataTypeProperty, value); }
    }

    // Using a DependencyProperty as the backing store for DragDropDataType.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty DragDropDataTypeProperty =
        DependencyProperty.Register("DragDropDataType", typeof(Type), typeof(ListBoxDragDropBehavior), new PropertyMetadata(null));




    protected override void OnAttached() {
        base.OnAttached();

        AssociatedObject.PreviewMouseLeftButtonDown += OnPreviewMouseLeftButtonDown;
        AssociatedObject.MouseMove += OnMouseMove;
        AssociatedObject.Drop += OnDrop;
        AssociatedObject.DragOver += OnDragOver;
    }



    protected override void OnDetaching() {
        AssociatedObject.PreviewMouseLeftButtonDown -= OnPreviewMouseLeftButtonDown;
        AssociatedObject.MouseMove -= OnMouseMove;
        AssociatedObject.Drop -= OnDrop;
        AssociatedObject.DragOver -= OnDragOver;

        base.OnDetaching();
    }

    private void OnDragOver(object sender, DragEventArgs e) {
        e.Effects = DragDropEffects.Move;
        e.Handled = true;
    }

    private void OnDrop(object sender, DragEventArgs e) {
        object droppedData = e.Data.GetData(DragDropDataType);

        if (droppedData != null) {
            var target = ((FrameworkElement)e.OriginalSource).DataContext;

            if (droppedData != null && target != null && !ReferenceEquals(droppedData, target)) {
                var viewModel = AssociatedObject.DataContext as IDragDropTarget;
                viewModel?.MoveItem(droppedData, target);
            }
        }
    }

    private void OnMouseMove(object sender, MouseEventArgs e) {
        Point mousePos = e.GetPosition(null);
        Vector diff = dragStartPoint - mousePos;

        if (e.LeftButton == MouseButtonState.Pressed &&
                       (Math.Abs(diff.X) > SystemParameters.MinimumHorizontalDragDistance ||
                       Math.Abs(diff.Y) > SystemParameters.MinimumVerticalDragDistance)) {

            ListBox listView = (ListBox)sender;
            ListBoxItem? listViewItem = FindAncestor<ListBoxItem>((DependencyObject)e.OriginalSource);

            if (listViewItem is null) {
                return;
            }

            object draggedItem = listView.ItemContainerGenerator.ItemFromContainer(listViewItem);
            if (draggedItem is null) {
                return;
            }

            DragDrop.DoDragDrop(listViewItem, draggedItem, DragDropEffects.Move);
        }
    }

    private void OnPreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e) {
        dragStartPoint = e.GetPosition(null);
    }

    private static T? FindAncestor<T>(DependencyObject current) where T : DependencyObject {
        while (current != null) {
            if (current is T) {
                return (T)current;
            }

            current = VisualTreeHelper.GetParent(current);
        }

        return null;
    }
}
