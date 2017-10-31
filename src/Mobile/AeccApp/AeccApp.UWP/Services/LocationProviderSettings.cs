using AeccApp.Core.Services;
using AeccApp.UWP.Services;
using System;
using Windows.System;
using Xamarin.Forms;

[assembly: Dependency(typeof(LocationProviderSettings))]
namespace AeccApp.UWP.Services
{
    public class LocationProviderSettings : ILocationProviderSettings
    {
        public async void OpenLocationProviderSettings()
        { 
             Launcher.LaunchUriAsync(new Uri("ms-settings:privacy-location"));
        }
    }
}
