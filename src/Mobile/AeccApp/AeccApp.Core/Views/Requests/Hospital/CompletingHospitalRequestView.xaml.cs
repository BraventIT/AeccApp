using AeccApp.Core.Messages;
using System;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.GoogleMaps;
using Xamarin.Forms.Xaml;

namespace AeccApp.Core.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class CompletingHospitalRequestView : BaseContentPage
	{
		public CompletingHospitalRequestView ()
        {
            InitializeComponent();
           
            map.UiSettings.ScrollGesturesEnabled = false;
            map.UiSettings.CompassEnabled = false;
            map.UiSettings.ZoomControlsEnabled = false;

            MessagingCenter.Subscribe<GeolocatorMessage>(this, string.Empty, MoveCameraMap);
        }

     
        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            MessagingCenter.Unsubscribe<GeolocatorMessage>(this, string.Empty);
        }

        public async void MoveCameraMap(GeolocatorMessage message)
        {
            var pinHospital = new Pin() { Label = "Tu selección", Position = message.Position };
            pinHospital.IsDraggable = false;
            switch (Device.OS)
            {
                case TargetPlatform.Android:
                    //   pinBravent.Icon = BitmapDescriptorFactory.FromBundle($"location_pin.png");
                    break;
                case TargetPlatform.iOS:
                    pinHospital.Icon = BitmapDescriptorFactory.FromBundle($"location_pin.png");
                    break;
                default:
                    pinHospital.Icon = BitmapDescriptorFactory.FromBundle($"Assets/location_pin.png");
                    break;
            }

            map.Pins.Add(pinHospital);
            var animState = await map.AnimateCamera(CameraUpdateFactory.NewPositionZoom(
                     message.Position, 16d), TimeSpan.FromSeconds(1));
        }
    }
}