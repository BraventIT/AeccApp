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
            
            map.InitialCameraUpdate = CameraUpdateFactory.NewPositionZoom(new Position(40.486683, -3.665183), 16d);
            var pinBravent = new Pin() { Label = "Hospital de pruebas técnicas Bravent", Position = new Position(40.486683, -3.665183) };
            pinBravent.IsDraggable = false;
            switch (Device.OS)
            {
                case TargetPlatform.Android:
                    pinBravent.Icon = BitmapDescriptorFactory.FromBundle($"location_pin_hospital_map.png");
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
            MoveCameraMap(new Position(40.486683, -3.665183));

        }
        public async void MoveCameraMap(Position toPosition)
        {
            var animState = await map.AnimateCamera(CameraUpdateFactory.NewPositionZoom(
                     toPosition, 14d), TimeSpan.FromSeconds(1));
        }



    }
}