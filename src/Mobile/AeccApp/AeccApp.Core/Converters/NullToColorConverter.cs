using System;
using System.Globalization;
using Xamarin.Forms;

namespace AeccApp.Core.Converters
{
    public class NullToColorConverter : BaseNotifyProperty, IValueConverter
    {

        public NullToColorConverter() : this(Color.Black, Color.Transparent)
        {

        }

        public NullToColorConverter(Color trueColor, Color falseColor)
        {
            ColorToNotNull = trueColor;
            ColorToNull = falseColor;
        }



        private Color _colorToNotNull;

        public Color ColorToNotNull
        {
            get { return _colorToNotNull; }
            set { Set(ref _colorToNotNull, value); }
        }

        private Color _colorToNull;

        public Color ColorToNull
        {
            get { return _colorToNull; }
            set { Set(ref _colorToNull, value); }
        }


        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            try
            {
                bool negateValue = false;
                if (parameter != null && parameter is string)
                    Boolean.TryParse(parameter as string, out negateValue);

                bool b = value != null;
                if (negateValue ^ b)
                    return ColorToNotNull;
                else
                    return ColorToNull;
            }
            catch
            {
                return ColorToNull;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
