using System;
using Ayls.WP8Toolkit.Multibinding;

namespace Ayls.WP8Toolkit.Converters
{
    public class MinWidthMultibindingConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            var minWidth = 0;
            foreach (var value in values)
            {
                var width = int.Parse(value.ToString());
                if (width > minWidth)
                    minWidth = width;
            }

            return minWidth;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
