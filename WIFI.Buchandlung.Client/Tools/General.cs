using System.Globalization;
using System.Windows.Data;
using System.Windows;

namespace WIFI.Buchandlung.Client.Tools
{
    public static class General : System.Object
    {
        public static bool AreStringsValid(params string[] strings)
        {
            foreach (var str in strings)
            {
                if (string.IsNullOrEmpty(str))
                {
                    return false;
                }
            }
            return true;
        }
    }
    public class AusleihZuRückgabeDatum : IValueConverter
    {
        public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (value is DateTime date)
            {
                DateTime postConvertDateTime = date.AddDays(14);
                return postConvertDateTime;

            }
            return value!;

        }

        public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            return DependencyProperty.UnsetValue;
        }
    }
}

