using AeccApp.Droid.Renderers;
using Android.Graphics.Drawables;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(Xamarin.Forms.Button), typeof(ButtonRender))]

namespace AeccApp.Droid.Renderers
{
    public class ButtonRender : ButtonRenderer
    {

        protected override void OnElementChanged(ElementChangedEventArgs<Xamarin.Forms.Button> e)
        {
            base.OnElementChanged(e);

            if (Control != null)
            {
                var roundableShape = new GradientDrawable();
                roundableShape.SetShape(ShapeType.Rectangle);
                roundableShape.SetStroke((int)Element.BorderWidth, Element.BorderColor.ToAndroid());
                roundableShape.SetColor(Element.BackgroundColor.ToAndroid());
                roundableShape.SetCornerRadius(Element.BorderRadius * Resources.DisplayMetrics.Density);
                Control.Background = roundableShape;
                Control.TransformationMethod = null;
                Control.Elevation = 0;
            }

        }
    }
}