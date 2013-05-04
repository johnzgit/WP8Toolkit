using System;
using System.Globalization;

namespace Ayls.WP8Toolkit.Multibinding
{
    /// <summary>
    /// Code sourced from http://www.thejoyofcode.com/MultiBinding_for_Silverlight_3.aspx
    /// </summary>
    public interface IMultiValueConverter
    {
        object Convert(object[] values, Type targetType, object parameter, CultureInfo culture);
        object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture);
    }
}
