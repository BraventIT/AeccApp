using AeccApp.Core.Services;
using AeccApp.Droid.Services;
using Android.Content;
using Xamarin.Forms;

[assembly: Dependency(typeof(LocationProviderSettings))]
namespace AeccApp.Droid.Services
{
    public class LocationProviderSettings : ILocationProviderSettings
    {
        public void OpenLocationProviderSettings()
        {
            Android.App.Application.Context.StartActivity(new Intent(Android.Provider.Settings.ActionLocationSourceSettings));
        }
    }
}