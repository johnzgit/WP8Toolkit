using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace Ayls.WP8Toolkit.Converters
{
    public abstract class FuncToVisibilityConverter : IValueConverter
    {
        private readonly Func<object, bool> _func;

        protected FuncToVisibilityConverter(Func<object, bool> func)
        {
            _func = func;
        }

        public object Convert(object value, Type targetType,
            object parameter, CultureInfo culture)
        {          
            return _func(value) ? Visibility.Visible : Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType,
            object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
