using System.Diagnostics;
using System.Globalization;
using System.Reflection;
using System.Windows.Controls;
using System.Windows.Data;

namespace HBLibrary.Wpf.Converters;
public class GridViewColumnWidthConverter : IMultiValueConverter {
    private static readonly PropertyInfo desiredWidthProperty;
    static GridViewColumnWidthConverter() {
        PropertyInfo? temp = typeof(GridViewColumn).GetProperty("DesiredWidth", BindingFlags.Instance | BindingFlags.NonPublic);
        Debug.Assert(temp is not null);

        desiredWidthProperty = temp;
    }

    public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture) {
        if (parameter is string param && param == "*" && values.Length == 3 && values[0] is double actualWidth && actualWidth > 0d) {
            if (values[1] is GridView gridView && values[2] is GridViewColumn column && desiredWidthProperty is not null) {
                double w = 0d;
                foreach (var col in gridView.Columns) {
                    if (col == column)
                        continue;

                    w += col.ActualWidth > 0 ? col.ActualWidth : (double)desiredWidthProperty.GetValue(col)!;
                }
                double desiredWidth = actualWidth - w;
                return desiredWidth > 100 ? desiredWidth - 5 /* scrollbar width */ : double.NaN;
            }
        }

        return double.NaN;
    }

    public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture) {
        return [];
    }
}
