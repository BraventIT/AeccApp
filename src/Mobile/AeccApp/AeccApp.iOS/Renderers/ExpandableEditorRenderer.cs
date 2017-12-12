using Xamarin.Forms.Platform.iOS;

namespace AeccApp.iOS.Renderers
{
    public class ExpandableEditorRenderer : EditorRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<Xamarin.Forms.Editor> e)
        {
            base.OnElementChanged(e);

            if (Control != null)
                Control.ScrollEnabled = false;
        }
    }
}