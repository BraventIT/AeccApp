using AeccApp.Core.IDependencyServices;
using Android.Content;
using Xamarin.Forms;

[assembly: Xamarin.Forms.Dependency(typeof(AeccApp.Droid.DependencyServices.LocationProviderSettings))]
namespace AeccApp.Droid.DependencyServices
{
    public class LocationProviderSettings : ILocationProviderSettings
    {
        public void OpenLocationProviderSettings()
        {
            Forms.Context.StartActivity(new Intent(Android.Provider.Settings.ActionLocationSourceSettings));
        }
    }
}