using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace GenshinWishCalculator.Helpers
{
    class TimerToResinConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                return null;

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
