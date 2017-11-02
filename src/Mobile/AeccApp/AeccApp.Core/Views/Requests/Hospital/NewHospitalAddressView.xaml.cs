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

            MessagingCenter.Subscribe<GeolocatorMessages, Position>(this, string.Empty, (sender, arg) =>
                MoveCameraMap(arg));
            InitializeComponent();
            map.InitialCameraUpdate = CameraUpdateFactory.NewPositionZoom(new Xamarin.Forms.GoogleMaps.Position(40.416937, -3.703523), 6d);
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            MessagingCenter.Unsubscribe<GeolocatorMessages>(this, string.Empty);
        }
        public async void MoveCameraMap(Position toPosition)
        {
            var animState = await map.AnimateCamera(CameraUpdateFactory.NewPositionZoom(toPosition
                    , 16d), TimeSpan.FromSeconds(1));
        }
    }
}