using AeccApp.UWP.CustomRenderer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Media;
using Xamarin.Forms;
using Xamarin.Forms.Platform.UWP;

[assembly: ExportRenderer(typeof(Frame), typeof(FrameRender))]
namespace AeccApp.UWP.CustomRenderer
{
    public class FrameRender : FrameRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<Frame> e)
        {
            base.OnElementChanged(e);
            var frame = e.NewElement;

            if (Control != null && frame.CornerRadius > 0)
            {
               

                Windows.UI.Color backgroundColor = Windows.UI.Color.FromArgb(
                    (byte)(frame.BackgroundColor.A * 255),
                    (byte)(frame.BackgroundColor.R * 255),
                    (byte)(frame.BackgroundColor.G * 255),
                    (byte)(frame.BackgroundColor.B * 255));

                Control.CornerRadius = new Windows.UI.Xaml.CornerRadius(frame.CornerRadius);
                Control.Background = new SolidColorBrush(backgroundColor);
                frame.BackgroundColor = Color.Transparent;
            }
        }
    }
}
