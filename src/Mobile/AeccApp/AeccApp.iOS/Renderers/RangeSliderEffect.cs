using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportEffect(typeof(AeccApp.iOS.Renderers.RangeSliderEffect), nameof(AeccApp.iOS.Renderers.RangeSliderEffect))]
namespace AeccApp.iOS.Renderers
{
    public class RangeSliderEffect:PlatformEffect
    {
        protected override void OnAttached()
        {
            var ctrl = (Xamarin.RangeSlider.RangeSliderControl)Control;
            ctrl.TintColor = Color.Fuchsia.ToUIColor();
            ctrl.ShowTextAboveThumbs = true;
            ctrl.LowerValue = 00.0f;
            ctrl.UpperValue = 100.0f;
        }

        protected override void OnDetached()
        {
            throw new System.NotImplementedException();
        }
    }
}
