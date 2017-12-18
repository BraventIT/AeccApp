using AeccApp.Core.Controls;
using AeccApp.UWP.CustomRenderer;
using Xamarin.Forms.Platform.UWP;
using Xamarin.Forms;

[assembly: ExportRenderer(typeof(BorderlessEntry), typeof(BorderlessEntryRenderer))]
namespace AeccApp.UWP.CustomRenderer
{
    public class BorderlessEntryRenderer : EntryRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<Entry> e)
        {
            base.OnElementChanged(e);

            if (Control != null)
            {
                Control.BorderThickness = new Windows.UI.Xaml.Thickness(0);
            }
        }
    }
}
