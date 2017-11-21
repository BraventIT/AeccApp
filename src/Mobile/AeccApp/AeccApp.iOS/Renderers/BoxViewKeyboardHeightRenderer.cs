using AeccApp.Core.Controls;
using AeccApp.iOS.Renderers;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(BoxViewKeyboardHeight), typeof(BoxViewKeyboardHeightRenderer))]
namespace AeccApp.iOS.Renderers
{
    public class BoxViewKeyboardHeightRenderer : BoxRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<BoxView> e)
        {
            base.OnElementChanged(e);

            if (Element != null)
            {
                Element.HeightRequest = 0;
            }

            UIKeyboard.Notifications.ObserveWillShow((sender, args) =>
            {
                if (Element != null)
                    Element.HeightRequest = args.FrameEnd.Height;
            });

            UIKeyboard.Notifications.ObserveWillHide((sender, args) =>
            {
                if (Element != null)
                    Element.HeightRequest = 0;
            });
        }
    }
}
