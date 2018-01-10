using AeccApp.iOS.Renderers;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(DatePicker), typeof(DatePickerRender))]
namespace AeccApp.iOS.Renderers
{
    public class DatePickerRender : DatePickerRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<DatePicker> e)
        {
            base.OnElementChanged(e);

            if (Control!=null)
            {
                Control.TextAlignment = UITextAlignment.Center;
            }
        }
    }
}