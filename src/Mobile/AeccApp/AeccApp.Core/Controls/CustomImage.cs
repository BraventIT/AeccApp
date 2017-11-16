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
            BindableProperty.Create(nameof(MaximumWidthRequest), typeof(double), typeof(CustomImage), double.NaN);


        public ImageSource SourcePlatform
        {
            get { return (ImageSource)GetValue(MaximumWidthRequestProperty); }
            set { SetValue(IsVisibleProperty, value); }
        }

        public static BindableProperty SourcePlatformProperty = BindableProperty.Create(
            nameof(SourcePlatform), 
            typeof(ImageSource), 
            typeof(CustomImage), 
            null, 
            propertyChanged: OnSourcePlatformChanged);

        private static void OnSourcePlatformChanged(BindableObject b, object oldValue, object newValue)
        {
            if (newValue == null)
                return;

            var customImage = (CustomImage)b;

            var source = (FileImageSource)newValue;

            string path = source.File;
            if (Device.RuntimePlatform == Device.UWP)
            {
                path = (path.EndsWith(".jpg", StringComparison.CurrentCultureIgnoreCase)) ?
                    $"Assets/{path}" : $"Assets/{path}.png";
            }

            source.File = path;
            customImage.Source = source;
        }

        public ImageSource SourcePlatform2
		{
			get
			{
				return (ImageSource)GetValue(SourceProperty);
			}
			set
			{
				if (value == null)
				{
					return;
				}

				var source = (FileImageSource)value;

				string path = source.File;
                if (Device.RuntimePlatform == Device.UWP)
                {
                    path = (path.EndsWith(".jpg", StringComparison.CurrentCultureIgnoreCase)) ?
                        $"Assets/{path}": $"Assets/{path}.png";
                }

				source.File = path;
				SetValue(SourceProperty, source);
			}
		}
	}
}
