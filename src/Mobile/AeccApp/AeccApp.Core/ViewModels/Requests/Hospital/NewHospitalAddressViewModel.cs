using AeccApi.Models;
using AeccApp.Core.Extensions;
using AeccApp.Core.Messages;
using AeccApp.Core.Models;
using AeccApp.Core.Services;
using Plugin.Geolocator.Abstractions;
using System.Collections.ObjectModel;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.GoogleMaps;
using System.Linq;
using System;
using AeccApp.Core.ViewModels.Popups;

namespace AeccApp.Core.ViewModels
{
    public class NewHospitalAddressViewModel : ViewModelBase
    {
        private IGeolocator GeolocatorService { get; } = ServiceLocator.GeolocatorService;
        private IHospitalRequestService HospitalRequestService { get; } = ServiceLocator.HospitalRequestService;
        private IMapPositionsDataService MapPositionsDataService { get; } = ServiceLocator.MapPositionsDataService;
        private IGoogleMapsService GoogleMapsService { get; } = ServiceLocator.GoogleMapsService;

        #region Contructor & Initialize
        public NewHospitalAddressViewModel()
        {
            _mapPins = new ObservableCollection<Pin>();
            _hospitals = new ObservableCollection<Hospital>();
            NoLocationProviderPopupVM = new NoLocationProviderPopupViewModel();
        }

        public override Task ActivateAsync()
        {
            return ExecuteOperationAsync(async cancelToken =>
           {
               if (GeolocatorService.IsGeolocationEnabled)
               {
                   var currentPosition = await GeolocatorService.GetCurrentLocationAsync(cancelToken);
                   AddressModel currentAddress = null;
                   if (currentPosition != null)
                   {
                       currentAddress = await GoogleMapsService.FindCoordinatesGeocodingAsync(currentPosition.Latitude, currentPosition.Longitude, cancelToken);
                       MessagingCenter.Send(new GeolocatorMessages(GeolocatorEnum.Refresh), string.Empty, currentPosition);
                   }

                   var currentProvince = (currentAddress != null) ? currentAddress.Province : string.Empty;
                   var hospitals = await HospitalRequestService.GetHospitalsAsync(currentProvince, cancelToken);
                   if (!hospitals.Any())
                       hospitals = await HospitalRequestService.GetHospitalsAsync(string.Empty, cancelToken);
                   foreach (var hospital in hospitals)
                   {
                       if (!string.IsNullOrEmpty(hospital.Street) && !string.IsNullOrEmpty(hospital.Name))
                       {
                           var location = await GetLocationForHospitalAsync(hospital, cancelToken);
                           if (location != null)
                               PinManagement(hospital.Street, hospital.Name, location.Latitude, location.Longitude);
                       }
                   }

                   //TODO uncomment this when line178 TO DO is done:

                   //var savedPins = await MapPinsDataService.GetListAsync();
                   //MapPins.AddRange(savedPins.Values);
               }
               else
               {
                   //Popup to open location settings
                   await NavigationService.ShowPopupAsync(NoLocationProviderPopupVM);
               }
           });
        }
        #endregion

        #region Properties

        public NoLocationProviderPopupViewModel NoLocationProviderPopupVM { get; private set; }


        private AddressModel _addressSelected = new AddressModel();

        public AddressModel AddressSelected
        {
            get { return _addressSelected; }
            set { _addressSelected = value; }
        }

        private ObservableCollection<Hospital> _hospitals;

        public ObservableCollection<Hospital> Hospitals
        {
            get { return _hospitals; }
        }

        private ObservableCollection<Pin> _mapPins;

        public ObservableCollection<Pin> MapPins
        {
            get { return _mapPins; }
        }

        private bool _switchBetweenAndHospitalList = true;

        public bool SwitchBetweenAndHospitalList
        {
            get { return _switchBetweenAndHospitalList; }
            set { Set(ref _switchBetweenAndHospitalList, value); }
        }

        #endregion

        #region Commands
        private Command _pinClickedCommand;
        public ICommand PinClickedCommand
        {
            get
            {
                return _pinClickedCommand ??
                    (_pinClickedCommand = new Command(OnPinClickedCommand, (o) => !IsBusy));
            }
        }

        async void OnPinClickedCommand(object obj)
        {
            var pinClicked = obj as Pin;
            AddressSelected.Street = pinClicked.Address;
            AddressSelected.Coordinates = new Xamarin.Forms.GoogleMaps.Position(pinClicked.Position.Latitude, pinClicked.Position.Longitude);
            AddressSelected.Name = pinClicked.Label;
            await NavigationService.NavigateToAsync<HospitalRequestChooseTypeViewModel>(AddressSelected);
        }

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

        async void OnHospitalListTabCommand(object obj)
        {
            SwitchBetweenAndHospitalList = false;
            await ExecuteOperationAsync(async cancelToken =>
            {
                if (!Hospitals.Any())
                {
                    var hospitals = await HospitalRequestService.GetHospitalsAsync(string.Empty, cancelToken);
                    Hospitals.AddRange(hospitals);
                }
            });
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
            var hospitalSelected = obj as Hospital;
            if (hospitalSelected == null)
                return;
            await ExecuteOperationAsync(async cancelToken =>
            {
                AddressSelected.Street = hospitalSelected.Street;
                AddressSelected.Name = hospitalSelected.Name;
                AddressSelected.Coordinates = await GetLocationForHospitalAsync(hospitalSelected, cancelToken);

                await NavigationService.NavigateToAsync<HospitalRequestChooseTypeViewModel>(AddressSelected);
            });
        }

        #endregion

        #region Methods
        private async Task<Xamarin.Forms.GoogleMaps.Position> GetLocationForHospitalAsync(Hospital hospital, CancellationToken cancelToken)
        {
            var hospitalAddress = $"{hospital.Name}, {hospital.Street}";

            var location = await MapPositionsDataService.GetAsync(hospitalAddress);
            if (location.Latitude == 0)
            {
                location = await GoogleMapsService.FindAddressGeocodingAsync(hospitalAddress, cancelToken);
                if (location.Latitude == 0)
                {
                    location = await GoogleMapsService.FindAddressGeocodingAsync($"{hospital.Name}, {hospital.Province}", cancelToken);
                    if (location.Latitude != 0)
                        await MapPositionsDataService.AddOrUpdateAsync(hospitalAddress, location);
                }
            }
            return location;
        }

        private void PinManagement(string hospitalAddress,string hospitalName, double lat, double lng)
        {
            Pin pin = new Pin() {Address = hospitalAddress, Label = hospitalName, Position = new Xamarin.Forms.GoogleMaps.Position(lat, lng) };
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
