using AeccApp.Core.Extensions;
using AeccApp.Core.Messages;
using AeccApp.Core.Services;
using Plugin.Geolocator.Abstractions;
using System.Windows.Input;
using Xamarin.Forms;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using Xamarin.Forms.GoogleMaps;
using System.Text.RegularExpressions;

namespace AeccApp.Core.ViewModels
{
    public class NewHospitalAddressViewModel : ViewModelBase
    {
        private IGeolocator GeolocatorService { get; } = ServiceLocator.GeolocatorService;
        private IHospitalRequestService HospitalRequestService { get; } = ServiceLocator.HospitalRequestService;
        private IMapPositionsDataService MapPositionsDataService { get; } = ServiceLocator.MapPositionsDataService;
        private IGoogleMapsService GoogleMapsService { get; } = ServiceLocator.GoogleMapsService;


        public async override Task ActivateAsync()
        {
            if (GeolocatorService.IsGeolocationEnabled)
            {
                var currentPosition = await GeolocatorService.GetCurrentLocationAsync();
                Models.AddressModel currentAddress = null;
                if (currentPosition != null)
                {
                    currentAddress = await GoogleMapsService.FindCoordinatesGeocodingAsync(currentPosition.Latitude, currentPosition.Longitude);
                    MessagingCenter.Send(new GeolocatorMessages(GeolocatorEnum.Refresh), string.Empty, currentPosition);
                }

                var currentProvince = (currentAddress != null) ? currentAddress.Province : string.Empty;
                var Hospitals = await HospitalRequestService.GetHospitalsAsync(currentProvince);
                foreach (var hospital in Hospitals)
                {
                    if (!string.IsNullOrEmpty(hospital.Street) && !string.IsNullOrEmpty(hospital.Name))
                    {
                        var hospitalAddress = $"{hospital.Name}, {hospital.Street}";

                        var location = await MapPositionsDataService.GetAsync(hospitalAddress);
                        if (location == null)
                        {
                            location = await GoogleMapsService.FindAddressGeocodingAsync(hospitalAddress);
                        }
                        if (location == null)
                        {
                            location = await GoogleMapsService.FindAddressGeocodingAsync(hospital.Name);
                        }
                        if (location != null)
                        {
                            await MapPositionsDataService.AddOrUpdateAddressAsync(hospitalAddress, location);
                            PinManagement(hospital.Name, location.Latitude, location.Longitude);
                        }
                    }
                }

                //TODO uncomment this when line178 TO DO is done:

                //var savedPins = await MapPinsDataService.GetListAsync();
                //MapPins.AddRange(savedPins.Values);

            }
            else
            {
                // TODO Mostrar Popup para decirle al usuario que no tiene activado la geolocalización.
            }
        }

        #region Commands


        private Command _hospitalMapTabCommand;
        public ICommand HospitalMapTabCommand
        {
            get
            {
                return _hospitalMapTabCommand ??
                    (_hospitalMapTabCommand = new Command(OnHospitalMapTabCommand, (o) => !IsBusy));
            }
        }

        void OnHospitalMapTabCommand(object obj)
        {
            SwitchBetweenAndHospitalList = true;
        }
        private Command _hospitalListTabCommand;
        public ICommand HospitalListTabCommand
        {
            get
            {
                return _hospitalListTabCommand ??
                    (_hospitalListTabCommand = new Command(OnHospitalListTabCommand, (o) => !IsBusy));
            }
        }

        void OnHospitalListTabCommand(object obj)
        {
            SwitchBetweenAndHospitalList = false;

        }



        private Command _newHospitalSelectedCommand;
        public ICommand NewHospitalSelectedCommand
        {
            get
            {
                return _newHospitalSelectedCommand ??
                    (_newHospitalSelectedCommand = new Command(OnNewHospitalSelectedCommand, (o) => !IsBusy));
            }
        }

        async void OnNewHospitalSelectedCommand(object obj)
        {
            await NavigationService.NavigateToAsync<NewRequestSelectAddressViewModel>();
        }


        private Command _relocateCommand;
        public ICommand RelocateCommand
        {
            get
            {
                return _relocateCommand ??
                    (_relocateCommand = new Command(OnRelocateCommand, (o) => !IsBusy));
            }
        }

        async void OnRelocateCommand(object obj)
        {
            if (GeolocatorService.IsGeolocationEnabled)
            {
                var position = await GeolocatorService.GetCurrentLocationAsync();
                MessagingCenter.Send(new GeolocatorMessages(GeolocatorEnum.Refresh), string.Empty, position);
            }
            else
            {
                // TODO Mostrar Popup para decirle al usuario que no tiene activado la geolocalización.
            }



        }



        #endregion

        #region Properties

        private ObservableCollection<Pin> _mapPins = new ObservableCollection<Pin>();

        public ObservableCollection<Pin> MapPins
        {
            get { return _mapPins; }
            set { Set(ref _mapPins, value); }
        }


        private bool _switchBetweenAndHospitalList = true;

        public bool SwitchBetweenAndHospitalList
        {
            get { return _switchBetweenAndHospitalList; }
            set { Set(ref _switchBetweenAndHospitalList, value); }
        }


        #endregion

        #region Methods
        private void PinManagement(string hospitalName, double lat, double lng)
        {
            Pin pin = new Pin() { Label = hospitalName, Position = new Xamarin.Forms.GoogleMaps.Position(lat, lng) };
            switch (Device.OS)
            {
                case TargetPlatform.Android:
                    // pinBravent.Icon = BitmapDescriptorFactory.FromBundle($"location_pin_hospital_map.png");
                    break;
                case TargetPlatform.iOS:
                    pin.Icon = BitmapDescriptorFactory.FromBundle($"location_pin_hospital_map.png");
                    break;
                default:
                    pin.Icon = BitmapDescriptorFactory.FromBundle($"Assets/location_pin_hospital_map.png");
                    break;
            }

            //TODO delete this line when next TO DO is fixed:
            MapPins.Add(pin);

            //TODO fix System.IO.IOException: Sharing violation on path /data/user/0/net.bravent.aeccapp/files/MapPins.json
            // await MapPinsDataService.AddOrUpdateAddressAsync(hospitalAddress, pin);
        }
        #endregion





    }
}
