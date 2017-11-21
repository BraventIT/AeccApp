using System;
using CoreGraphics;
using PrismForms.iOS.Effects;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;
 
[assembly: ResolutionGroupName("EntryEffects")]
[assembly: ExportEffect(typeof(EntryEffect), "EntryEffect")]
namespace PrismForms.iOS.Effects
{
    public class EntryEffect : PlatformEffect
    {
        protected override void OnAttached()
        {
            var entry = (UITextField)Control;
            entry.InputAccessoryView = BuildDismiss();
        }

        protected override void OnDetached()
        {

        }

        private UIToolbar BuildDismiss()
        {
            var toolbar = new UIToolbar(new CGRect(0.0f, 0.0f, Control.Frame.Size.Width, 44.0f));

            toolbar.Items = new[]
            {
                new UIBarButtonItem(UIBarButtonSystemItem.FlexibleSpace),
                new UIBarButtonItem(UIBarButtonSystemItem.Done, delegate { Control.ResignFirstResponder(); })
            };

            return toolbar;
        }
    }
}
