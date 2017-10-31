using AeccApp.Core.IDependencyServices;
using Xamarin.Forms;

[assembly: Xamarin.Forms.Dependency(typeof(AeccApp.UWP.DependencyServices.LocationProviderSettings))]
namespace AeccApp.UWP.DependencyServices
{
    public class LocationProviderSettings : ILocationProviderSettings
    {
        public async void OpenLocationProviderSettings()
        { 
             Windows.System.Launcher.LaunchUriAsync(new System.Uri("ms-settings:privacy-location"));
        }
    }
}
