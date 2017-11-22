using AeccApp.Core.Extensions;
using System;
using System.Globalization;
using Xamarin.Forms;

namespace AeccApp.Core.Converters
{
	public class ImagePlatformConverter : IValueConverter
	{
        /// <summary>
        /// Converts plain string to an usable ImagePlatform string that depends on the platform
        /// </summary>

		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if (string.IsNullOrEmpty(value?.ToString()))
			{
				return value;
			}

			return SourceExtensions.GetPathFixed((string)value);
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}
