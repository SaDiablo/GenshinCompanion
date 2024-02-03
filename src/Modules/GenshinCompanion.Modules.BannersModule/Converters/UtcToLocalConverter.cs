using System;
using System.Globalization;
using System.Windows.Data;

namespace GenshinCompanion.Modules.BannersModule.Converters
{
    public class UtcToLocalConverter : IValueConverter
    {
        public object Convert(object value, System.Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
            {
                return null;
            }

            if (value is DateTime dateTime)
            {
                return dateTime.ToLocalTime();
            }

            return Binding.DoNothing;
        }

        public object ConvertBack(object value, System.Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
            {
                return null;
            }

            if (value is DateTime dateTime)
            {
                return dateTime.ToUniversalTime();
            }

            return Binding.DoNothing;
        }
    }
}