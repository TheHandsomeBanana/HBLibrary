﻿using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace HBLibrary.Wpf.Converters;
public class IntToVisibilityConverter : IValueConverter {
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture) {
        return (value is int iVal && iVal == -1) ? Visibility.Collapsed : Visibility.Visible;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
        throw new NotImplementedException();
    }
}
