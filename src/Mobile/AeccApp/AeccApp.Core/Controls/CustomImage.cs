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
                if (Device.RuntimePlatform == Device.Windows)
				{
					path = $"Assets/{path}.png";
				}

				source.File = path;
				SetValue(SourceProperty, source);
			}
		}
	}
}
