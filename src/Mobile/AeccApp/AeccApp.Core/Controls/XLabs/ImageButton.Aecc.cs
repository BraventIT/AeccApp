using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace XLabs.Forms.Controls
{
    public partial class ImageButton : Button
    {
        public ImageSource SourcePlatform
        {
            get { return (ImageSource)GetValue(SourceProperty); }
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
                    path = $"Assets/{path}.png";
                }

                source.File = path;
                SetValue(SourceProperty, source);
            }
        }
    }
}
