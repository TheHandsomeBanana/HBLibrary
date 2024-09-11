using System.Globalization;
using System.Windows.Data;

namespace HBLibrary.Wpf.Converters;
public class ValidationErrorConverter : IMultiValueConverter {
    public object? Convert(object[] values, Type targetType, object parameter, CultureInfo cultureInfo) {
        if (values != null && values.Length == 2) {
            var error = values[0] as string;
            var hasError = (bool)values[1];

            if (hasError) {
                return error;
            }
        }

        return string.Empty;
    }

    public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo cultureInfo) {
        throw new NotImplementedException();
    }
}