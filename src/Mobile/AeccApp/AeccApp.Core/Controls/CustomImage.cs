using AeccApp.Core.Extensions;
using System;
using Xamarin.Forms;

namespace AeccApp.Core.Controls
{
    public class CustomImage : Image
    {
        /// <summary>
        /// Custom control made to load assets managing UWP folder (Assets/), iOS and Android at the same time.
        /// </summary>

        public CustomImage()
        {
            SizeChanged += CustomImage_SizeChanged;
        }

        private void CustomImage_SizeChanged(object sender, EventArgs e)
        {
            if (Width > MaximumWidthRequest)
            {
                WidthRequest = MaximumWidthRequest;
            }
        }

        public double MaximumWidthRequest
        {
            get { return (double)GetValue(MaximumWidthRequestProperty); }
            set { SetValue(IsVisibleProperty, value); }
        }

        public static BindableProperty MaximumWidthRequestProperty =
            BindableProperty.Create(
                nameof(MaximumWidthRequest), 
                typeof(double), 
                typeof(CustomImage), 
                double.NaN);


        public string SourcePlatform
        {
            get { return (string)GetValue(SourcePlatformProperty); }
            set { SetValue(SourcePlatformProperty, value); }
        }

        public static BindableProperty SourcePlatformProperty = BindableProperty.Create(
            nameof(SourcePlatform),
            typeof(string),
            typeof(CustomImage),
            null,
            propertyChanged: OnSourcePlatformChanged);

        private static void OnSourcePlatformChanged(BindableObject b, object oldValue, object newValue)
        {
            if (newValue == null)
                return;
            var control = (CustomImage)b;
            control.SetSourcePlatform((string)newValue);
        }
    }
}
