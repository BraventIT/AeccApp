using AeccApp.Core.Messages;
using System;
using Xamarin.Forms;
using Xamarin.Forms.GoogleMaps;
using Xamarin.Forms.Xaml;

namespace AeccApp.Core.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class NewHospitalAddressView : BaseContentPage
	{
		public NewHospitalAddressView ()
		{
            MessagingCenter.Subscribe<GeolocatorMessages, Position>(this, string.Empty, (sender, arg) => 
                MoveCameraMap(arg));
			InitializeComponent ();
            map.InitialCameraUpdate = CameraUpdateFactory.NewPositionZoom(new Position(40.416937, -3.703523), 6d);
            var pinBravent = new Pin() { Label = "Hospital de pruebas técnicas Bravent", Position = new Position(40.416937, -3.703523) };
            pinBravent.IsDraggable = false;
            switch (Device.OS)
            {
                case TargetPlatform.Android:
                   // pinBravent.Icon = BitmapDescriptorFactory.FromBundle($"location_pin_hospital_map.png");
                    break;
                case TargetPlatform.iOS:
                    pinBravent.Icon = BitmapDescriptorFactory.FromBundle($"location_pin_hospital_map.png");
                    break;
                default:
                    pinBravent.Icon = BitmapDescriptorFactory.FromBundle($"Assets/location_pin_hospital_map.png");
                    break;
            }

            map.Pins.Add(pinBravent);
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            MessagingCenter.Unsubscribe<GeolocatorMessages>(this, string.Empty);
        }
        public async void MoveCameraMap(Position toPosition)
        {
            var animState = await map.AnimateCamera(CameraUpdateFactory.NewPositionZoom(
                     toPosition, 16d), TimeSpan.FromSeconds(1));
        }

     

    }
}