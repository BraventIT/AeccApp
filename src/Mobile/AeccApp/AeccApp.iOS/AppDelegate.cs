using Foundation;
using Microsoft.Identity.Client;
using UIKit;

namespace AeccApp.iOS
{
    [Register("AppDelegate")]
    public partial class AppDelegate : global::Xamarin.Forms.Platform.iOS.FormsApplicationDelegate
    {
        private const string GoogleIOSMapKey = "AIzaSyCYrC9dTd2tLpBuPEbFqmmJHyO-hJywvFk";

        public override bool FinishedLaunching(UIApplication app, NSDictionary options)
        {
            global::Xamarin.Forms.Forms.Init();
            Xamarin.FormsGoogleMaps.Init(GoogleIOSMapKey);
            LoadApplication(new App());

            return base.FinishedLaunching(app, options);
        }

		public override bool OpenUrl(UIApplication app, NSUrl url, NSDictionary options)
		{
			AuthenticationContinuationHelper.SetAuthenticationContinuationEventArgs(url);
			return true;
		}

    }
}
