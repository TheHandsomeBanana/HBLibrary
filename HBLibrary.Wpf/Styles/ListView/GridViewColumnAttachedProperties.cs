using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;

namespace HBLibrary.Wpf.Styles.ListView;
public static class GridViewColumnAttachedProperties {
    public static readonly DependencyProperty MinWidthProperty =
        DependencyProperty.RegisterAttached(
            "MinWidth",
            typeof(double),
            typeof(GridViewColumnAttachedProperties),
            new PropertyMetadata(double.NaN, OnMinWidthChanged));

    public static void SetMinWidth(GridViewColumn element, double value) {
        element.SetValue(MinWidthProperty, value);
    }

    public static double GetMinWidth(GridViewColumn element) {
        return (double)element.GetValue(MinWidthProperty);
    }

    private static void OnMinWidthChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) {
        if (d is GridViewColumn column) {
            SetMinWidth(column);
            DependencyPropertyDescriptor dpd = DependencyPropertyDescriptor.FromProperty(GridViewColumn.WidthProperty, typeof(GridViewColumn));

            dpd?.AddValueChanged(column, (sender, args) => {
                SetMinWidth(column);
            });
        }
    }

    private static void SetMinWidth(GridViewColumn column) {
        double minWidth = (double)column.GetValue(MinWidthProperty);

        if (column.Width < minWidth)
            column.Width = minWidth;
    }
}
