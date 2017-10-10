using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace AeccApp.Core.Converters
{
    public class TextChangeEventArgsConverter : IValueConverter
    {
        /// <summary>
        /// TextChangedEventArgs converter, returns string with current text 
        /// </summary>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var eventArgs = value as TextChangedEventArgs;

            if (eventArgs == null)
                throw new ArgumentException("Expected TextChangedEventArgs as value", "value");

            return eventArgs.NewTextValue;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
