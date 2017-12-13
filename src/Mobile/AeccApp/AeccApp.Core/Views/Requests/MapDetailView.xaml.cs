using AeccApp.Core.Messages;
using System;
using Xamarin.Forms;
using Xamarin.Forms.GoogleMaps;
using Xamarin.Forms.Xaml;

namespace AeccApp.Core.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MapDetailView : BaseContentPage
    {
        public MapDetailView()
        {
            InitializeComponent();
            map.InitialCameraUpdate = CameraUpdateFactory.NewPositionZoom(new Position(40.416937, -3.703523), 6d);
            MessagingCenter.Subscribe<GeolocatorMessage>(this, string.Empty, MoveCameraMap);
        }


        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            MessagingCenter.Unsubscribe<GeolocatorMessage>(this, string.Empty);
        }

        public async void MoveCameraMap(GeolocatorMessage message)
        {
            var addressPin = new Pin() { Label = "Tu dirección", Position = message.Position };
            addressPin.IsDraggable = false;
            switch (Device.OS)
            {
                case TargetPlatform.Android:
                    addressPin.Icon = BitmapDescriptorFactory.FromBundle($"map_location_pin_green.png");
                    break;
                case TargetPlatform.iOS:
                    addressPin.Icon = BitmapDescriptorFactory.FromBundle($"map_location_pin_green.png");
                    break;
                default:
                    addressPin.Icon = BitmapDescriptorFactory.FromBundle($"Assets/map_location_pin_green.png");
                    break;
            }

            map.Pins.Add(addressPin);
            var animState = await map.AnimateCamera(CameraUpdateFactory.NewPositionZoom(
                     message.Position, 16d), TimeSpan.FromSeconds(1));
        }
    }
}