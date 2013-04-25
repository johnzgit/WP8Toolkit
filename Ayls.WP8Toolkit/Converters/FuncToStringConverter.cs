using System;
using System.Globalization;
using System.Windows.Data;

namespace Ayls.WP8Toolkit.Converters
{
    public class FuncToStringConverter : IValueConverter
    {
        private readonly Func<object, string> _func;

        public FuncToStringConverter(Func<object, string> func)
        {
            _func = func;
        }

        public object Convert(object value, Type targetType,
            object parameter, CultureInfo culture)
        {          
            return _func(value);
        }

        public object ConvertBack(object value, Type targetType,
            object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
