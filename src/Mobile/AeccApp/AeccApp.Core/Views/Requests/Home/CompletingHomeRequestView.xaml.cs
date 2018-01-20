using AeccApp.Core.Messages;
using System;
using Xamarin.Forms;
using Xamarin.Forms.GoogleMaps;
using Xamarin.Forms.Xaml;

namespace AeccApp.Core.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class CompletingHomeRequestView : BaseContentPage
	{
        public CompletingHomeRequestView()
        {
            InitializeComponent();

            map.UiSettings.ScrollGesturesEnabled = false;
            map.UiSettings.CompassEnabled = false;
            map.UiSettings.ZoomControlsEnabled = false;
            map.InitialCameraUpdate = CameraUpdateFactory.NewPositionZoom(new Position(40.416937, -3.703523), 6d);
            MessagingCenter.Subscribe<GeolocatorMessage>(this, string.Empty, MoveCameraMap);
        }

   
        protected override void OnDisappearing()
        {
            base.OnDisappearing();
           // map.IsVisible = false;
            MessagingCenter.Unsubscribe<GeolocatorMessage>(this,string.Empty);
        }

        public async void MoveCameraMap(GeolocatorMessage message)
        {
            var addressPin = new Pin() { Label = "Tu dirección", Position = message.Position };
            addressPin.IsDraggable = false;
            switch (Device.RuntimePlatform)
            {
                case Device.Android:
                    addressPin.Icon = BitmapDescriptorFactory.FromBundle($"map_location_pin_green.png");
                    break;
                case Device.iOS:
                    addressPin.Icon = BitmapDescriptorFactory.FromBundle($"map_location_pin_green.png");
                    break;
                case Device.UWP:
                    addressPin.Icon = BitmapDescriptorFactory.FromBundle($"Assets/map_location_pin_green.png");
                    break;
                default:
                    break;
            }

            map.Pins.Add(addressPin);

            await map.AnimateCamera(CameraUpdateFactory.NewPositionZoom(
                     message.Position, 16d), TimeSpan.FromSeconds(1));
        }
    }
}