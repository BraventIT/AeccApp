using AeccApp.Core.Resources;
using System;
using System.Globalization;
using Xamarin.Forms;

namespace AeccApp.Core.Converters
{
    public class BooleanToColorConverter : BaseNotifyProperty, IValueConverter
    {

        public BooleanToColorConverter() : this(Color.White, Color.Black)
        {

        }

        public BooleanToColorConverter(Color trueColor, Color falseColor)
        {
            ColorToTrue = trueColor;
            ColorToFalse = falseColor;
        }

        private Color _colorToTrue;

        public Color ColorToTrue
        {
            get { return _colorToTrue; }
            set { Set(ref _colorToTrue, value); }
        }

        private Color _colorToFalse;

        public Color ColorToFalse
        {
            get { return _colorToFalse; }
            set { Set(ref _colorToFalse, value); }
        }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            try
            {
                bool negateValue = false;
                if (parameter != null && parameter is string)
                    Boolean.TryParse(parameter as string, out negateValue);

                bool b = (bool)value;
                if (negateValue ^ b)
                    return ColorToTrue;
                else
                    return ColorToFalse;
            }
            catch
            {
                return ColorToFalse;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
