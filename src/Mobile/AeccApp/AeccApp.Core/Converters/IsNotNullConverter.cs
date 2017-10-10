using System;
using System.Globalization;
using Xamarin.Forms;

namespace AeccApp.Core.Converters
{
    class IsNotNullConverter : IValueConverter
    {
        /// <summary>
        /// Converter returns true if value is not null
        /// </summary>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
            => value != null;

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
