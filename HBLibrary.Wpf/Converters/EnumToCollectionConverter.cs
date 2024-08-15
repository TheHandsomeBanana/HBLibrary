using System.Globalization;
using System.Windows.Data;

namespace HBLibrary.Wpf.Converters;
public class EnumToCollectionConverter : IValueConverter {
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture) {
        return Enum.GetValues(value.GetType()).Cast<Enum>().ToList();
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
        throw new NotImplementedException();
    }
}