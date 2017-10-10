using System;
using System.Globalization;
using Xamarin.Forms;

namespace AeccApp.Core.Converters
{
    public class InverseBoolConverter : IValueConverter
    {
        
        /// <summary>
        /// Converts bool on !bool
        /// </summary>

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(value is bool))
            {
                throw new InvalidOperationException("The target must be a boolean");
            }

            return !(bool)value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}
