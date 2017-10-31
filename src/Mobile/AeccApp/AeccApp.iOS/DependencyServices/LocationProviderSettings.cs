using AeccApp.Core.IDependencyServices;
using Foundation;
using UIKit;
using Xamarin.Forms;

[assembly: Xamarin.Forms.Dependency(typeof(AeccApp.iOS.DependencyServices.LocationProviderSettings))]
namespace AeccApp.iOS.DependencyServices
{
    public class LocationProviderSettings : ILocationProviderSettings
    {
        public void OpenLocationProviderSettings()
        {
           UIApplication.SharedApplication.OpenUrl(new NSUrl(UIKit.UIApplication.OpenSettingsUrlString));
        }
    }
}
