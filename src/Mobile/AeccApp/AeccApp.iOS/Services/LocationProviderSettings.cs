using AeccApp.Core.Services;
using AeccApp.iOS.Services;
using Foundation;
using UIKit;
using Xamarin.Forms;

[assembly: Dependency(typeof(LocationProviderSettings))]
namespace AeccApp.iOS.Services
{
    public class LocationProviderSettings : ILocationProviderSettings
    {
        public void OpenLocationProviderSettings()
        {
           UIApplication.SharedApplication.OpenUrl(new NSUrl(UIKit.UIApplication.OpenSettingsUrlString));
        }
    }
}
