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
        public NewHospitalAddressView()
        {
            InitializeComponent();

            MessagingCenter.Subscribe<GeolocatorMessage, Models.Position>(this, string.Empty, (sender, arg) => MoveCameraMap(arg));
            map.InitialCameraUpdate = CameraUpdateFactory.NewPositionZoom(new Position(40.416937, -3.703523), 6d);
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            MessagingCenter.Unsubscribe<GeolocatorMessage>(this, string.Empty);
        }
        public async void MoveCameraMap(Models.Position toPosition)
        {
            map.MyLocationEnabled = true;
            var animState = await map.AnimateCamera(CameraUpdateFactory.NewPositionZoom(
                    new Position(toPosition.Latitude, toPosition.Longitude)
                    , 16d), TimeSpan.FromSeconds(1));
        }
    }
}