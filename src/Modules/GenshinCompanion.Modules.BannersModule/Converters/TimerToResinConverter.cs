using System;
using System.Globalization;
using System.Windows.Data;

namespace GenshinCompanion.Modules.BannersModule.Converters
{
    public class TimerToResinConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
            {
                return null;
            }

            if (!(value is TimeSpan))
            {
                return Binding.DoNothing;
            }

            int resin = (int)((new TimeSpan(21, 20, 00) - (TimeSpan)value).TotalMinutes / 8);
            return resin;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}