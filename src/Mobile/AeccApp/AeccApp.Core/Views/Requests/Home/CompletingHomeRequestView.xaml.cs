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

            MessagingCenter.Subscribe<GeolocatorMessage>(this, string.Empty, MoveCameraMap);
        }

        private void Map_InfoWindowClicked(object sender, InfoWindowClickedEventArgs e)
        {
            throw new NotImplementedException();
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            MessagingCenter.Unsubscribe<GeolocatorMessage>(this,string.Empty);
        }

        public async void MoveCameraMap(GeolocatorMessage message)
        {
            var addressPin = new Pin() { Label = "Tu dirección", Position = message.Position };
            addressPin.IsDraggable = false;
            switch (Device.OS)
            {
                case TargetPlatform.Android:
                    // addressPin.Icon = BitmapDescriptorFactory.FromBundle($"location_pin.png");
                    break;
                case TargetPlatform.iOS:
                    addressPin.Icon = BitmapDescriptorFactory.FromBundle($"location_pin.png");
                    break;
                default:
                    addressPin.Icon = BitmapDescriptorFactory.FromBundle($"Assets/location_pin.png");
                    break;
            }

            map.Pins.Add(addressPin);

            var animState = await map.AnimateCamera(CameraUpdateFactory.NewPositionZoom(
                     message.Position, 16d), TimeSpan.FromSeconds(1));
        }
    }
}