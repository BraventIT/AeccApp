using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Xamarin.Forms;
using AeccApp.Core.Controls;
using AeccApp.Droid.Renderers;
using Xamarin.Forms.Platform.Android;
using Android.Graphics.Drawables;

[assembly: ExportRenderer(typeof(CustomButton), typeof(CustomButtonRender))]

namespace AeccApp.Droid.Renderers
{
    public class CustomButtonRender : ButtonRenderer
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