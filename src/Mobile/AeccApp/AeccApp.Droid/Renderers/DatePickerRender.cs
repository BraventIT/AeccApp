using AeccApp.Droid.Renderers;
using Android.Content;
using Android.Views;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(DatePicker), typeof(DatePickerRender))]

namespace AeccApp.Droid.Renderers
{
    public class DatePickerRender: DatePickerRenderer
    {
        public DatePickerRender(Context context) : base(context)
        {
        }

        protected override void OnElementChanged(ElementChangedEventArgs<DatePicker> e)
        {
            base.OnElementChanged(e);

            if (Control!=null)
            {
                Control.Gravity = GravityFlags.CenterHorizontal;
            }
        }
    }
}