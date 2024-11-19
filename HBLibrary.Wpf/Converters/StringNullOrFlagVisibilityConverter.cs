using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace HBLibrary.Wpf.Converters;
public class StringNullOrFlagVisibilityConverter : IMultiValueConverter {
    public object? Convert(object[] values, Type targetType, object parameter, CultureInfo culture) {
        if (values == null || values.Length < 2) {
            return null;
        }

        string? str = values[0]?.ToString();
        bool showCategory = values[1] as bool? ?? false;

        return str is not null && showCategory
            ? Visibility.Visible
            : Visibility.Collapsed;
    }

    public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture) {
        throw new NotImplementedException();
    }
}
