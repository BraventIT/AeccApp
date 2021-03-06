﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using Xamarin.Forms;

namespace AeccApp.Core.Converters
{
   public class InfoWindowClickedEventArgsConverter : IValueConverter
    {

        /// <summary>
        /// ItemTappedEventArgs converter, returns item tapped.
        /// </summary>

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var eventArgs = value as Xamarin.Forms.GoogleMaps.InfoWindowClickedEventArgs;
            if (eventArgs == null)
                throw new ArgumentException("Expected InfoWindowClickedEventArgs as value", "value");

            return eventArgs.Pin;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}