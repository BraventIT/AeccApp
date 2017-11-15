using AeccApp.Core.Messages;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Android.Views;
using Android.Widget;
using Microsoft.Identity.Client;
using Plugin.Permissions;
using Xamarin;
using Xamarin.Forms;

namespace AeccApp.Droid
{
    
    [Activity(Label = "AECC", Icon = "@drawable/icon", Theme = "@style/MyTheme", MainLauncher = true, ScreenOrientation = ScreenOrientation.Portrait, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        protected override void OnCreate(Bundle bundle)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(bundle);

            Forms.Init(this, bundle);
            FormsGoogleMaps.Init(this, bundle);

            LoadApplication(new App());
            App.UiParent = new UIParent(Forms.Context as Activity);


            // Only logo visible in dashboard page
            MessagingCenter.Subscribe<ToolbarMessage>(this, string.Empty, m =>
            {
                var logo = FindViewById<ImageView>(Resource.Id.LogoImageLayout);

                logo.Visibility = (m.ShowLogo) ?
                    ViewStates.Visible : ViewStates.Invisible;
            });
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, Permission[] grantResults)
        {
            PermissionsImplementation.Current.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }

        protected override void OnActivityResult(int requestCode, Result resultCode, Intent data)
        {
            base.OnActivityResult(requestCode, resultCode, data);
            AuthenticationContinuationHelper.SetAuthenticationContinuationEventArgs(requestCode, resultCode, data);
        }
    }
}
