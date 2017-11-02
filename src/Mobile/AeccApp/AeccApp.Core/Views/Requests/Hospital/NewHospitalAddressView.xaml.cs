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

            MessagingCenter.Subscribe<GeolocatorMessage>(this, string.Empty, MoveCameraMap);
            map.InitialCameraUpdate = CameraUpdateFactory.NewPositionZoom(new Position(40.416937, -3.703523), 6d);
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            MessagingCenter.Unsubscribe<GeolocatorMessage>(this, string.Empty);
        }
        public async void MoveCameraMap(GeolocatorMessage message)
        {
            map.MyLocationEnabled = true;
            map.UiSettings.MyLocationButtonEnabled = true;

            var animState = await map.AnimateCamera(CameraUpdateFactory.NewPositionZoom(message.Position, 16d), TimeSpan.FromSeconds(1));
        }
    }
}