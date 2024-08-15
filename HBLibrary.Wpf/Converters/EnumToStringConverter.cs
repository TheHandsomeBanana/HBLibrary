using System.Globalization;
using System.Text.RegularExpressions;
using System.Windows.Data;

namespace HBLibrary.Wpf.Converters;
public class EnumToStringConverter : IValueConverter {
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture) {
        if (value is null) {
            return "";
        }

        string enumValue = value.ToString()!;

        // Insert a space before every capital letter;
        string formattedValue = Regex.Replace(enumValue, "(?<!^)([A-Z])", " $1");
        return formattedValue;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
        throw new NotImplementedException();
    }
}
