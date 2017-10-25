using AeccApp.Core.Messages;
using AeccApp.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
			InitializeComponent ();
            
            map.InitialCameraUpdate = CameraUpdateFactory.NewPositionZoom(new Xamarin.Forms.GoogleMaps.Position(40.486683, -3.665183), 13d);
            var pinBravent = new Pin() { Label = "Hospital de pruebas técnicas Bravent", Position = new Xamarin.Forms.GoogleMaps.Position(40.486683, -3.665183) };
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


        protected override void OnAppearing()
        {
            base.OnAppearing();

            MessagingCenter.Subscribe<GeolocatorMessages, Xamarin.Forms.GoogleMaps.Position>(this, string.Empty, (sender, arg) => {
                MoveCameraMap(arg);
            });

           // MessagingCenter.Subscribe<GeolocatorMessages>(this, string.Empty, OnCoordinatesRefresh);
            MoveCameraMap(new Xamarin.Forms.GoogleMaps.Position(40.486683, -3.665183));

        }
        public async void MoveCameraMap(Xamarin.Forms.GoogleMaps.Position toPosition)
        {
            var animState = await map.AnimateCamera(CameraUpdateFactory.NewPositionZoom(
                     toPosition, 16d), TimeSpan.FromSeconds(1));
        }

     

    }
}