using AeccApp.Core.Resources;
using System;
using System.Globalization;
using Xamarin.Forms;

namespace AeccApp.Core.Converters
{
    public class GenderToImageConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (value?.ToString().StartsWith("h", StringComparison.CurrentCultureIgnoreCase) ?? true) ?
               ResourcesReference.MAN_ICON : ResourcesReference.GIRL_ICON;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
