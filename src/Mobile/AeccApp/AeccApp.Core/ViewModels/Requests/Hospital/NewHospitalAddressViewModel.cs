using Aecc.Models;
using AeccApp.Core.Extensions;
using AeccApp.Core.Messages;
using AeccApp.Core.Models;
using AeccApp.Core.Services;
using AeccApp.Core.ViewModels.Popups;
using Plugin.Geolocator.Abstractions;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.GoogleMaps;

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
                Xamarin.Forms.GoogleMaps.Position currentPosition = new Xamarin.Forms.GoogleMaps.Position();

                if (GeolocatorService.IsLocationAvailable())
                {
                    currentPosition = await GeolocatorService.GetCurrentLocationAsync(cancelToken);
                }
                AddressModel currentAddress = null;
                if (currentPosition.Latitude != 0)
                {
                    currentAddress = await GoogleMapsService.FindCoordinatesGeocodingAsync(currentPosition.Latitude, currentPosition.Longitude, cancelToken);
                    MessagingCenter.Send(new GeolocatorMessage(currentPosition), string.Empty);
                }
                else
                {
                    //Popup to open location settings
                    await NavigationService.ShowPopupAsync(NoLocationProviderPopupVM);
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
            });
        }
        #endregion

        #region Properties


        public NoLocationProviderPopupViewModel NoLocationProviderPopupVM { get; private set; }

        private string _addressFinder = string.Empty;
        public string AddressFinder
        {
            get { return _addressFinder; }
            set { Set(ref _addressFinder, value); }
        }

        private bool _isSearchIconVisible;
        public bool IsSearchIconVisible
        {
            get { return _isSearchIconVisible; }
            set { Set(ref _isSearchIconVisible, value); }
        }

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
            set { _hospitals = value; }

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

        private Command _resetAddressFinderCommand;
        public ICommand ResetAddressFinderCommand
        {
            get
            {
                return _resetAddressFinderCommand ??
                    (_resetAddressFinderCommand = new Command(o => OnResetAddressFinder()));
            }
        }

        private async void OnResetAddressFinder()
        {
            AddressSelected = null;
            AddressFinder = string.Empty;
            IsSearchIconVisible = false;
            Hospitals.Clear();
            await ExecuteOperationAsync(async cancelToken =>
            {
                if (!Hospitals.Any())
                {
                    var hospitals = await HospitalRequestService.GetHospitalsAsync(string.Empty, cancelToken);
                    Hospitals.AddRange(hospitals);
                }
            });

        }


        private Command _addressChangedCommand;
        public ICommand AddressChangedCommand
        {
            get
            {
                return _addressChangedCommand ??
                    (_addressChangedCommand = new Command(OnAddressChanged));
            }
        }

        private async void OnAddressChanged(object obj)
        {

            string result = string.Empty;
            if (obj is string)
            {
                result = (string)obj;
            }

            if (result.Length > 7)
            {

                if (string.IsNullOrWhiteSpace(result))
                {
                    IsSearchIconVisible = false;
                    Hospitals.Clear();
                    await ExecuteOperationAsync(async cancelToken =>
                    {
                        if (!Hospitals.Any())
                        {
                            var hospitals = await HospitalRequestService.GetHospitalsAsync(string.Empty, cancelToken);
                            Hospitals.AddRange(hospitals);
                        }
                    });
                }
                else
                {
                    RefreshHospitalList(result);
                    IsSearchIconVisible = true;
                }
            }
        }

        private Command _infoWindowClickedCommand;
        public ICommand InfoWindowClickedCommand
        {
            get
            {
                return _infoWindowClickedCommand ??
                    (_infoWindowClickedCommand = new Command(o => OnInfoWindowClickedCommand(o)));
            }
        }

        async Task OnInfoWindowClickedCommand(object obj)
        {
            var pinClicked = obj as Pin;

            AddressSelected.Street = pinClicked.Address;
            AddressSelected.Coordinates = pinClicked.Position;
            AddressSelected.Name = pinClicked.Label;
            await NavigationService.NavigateToAsync<HospitalRequestChooseTypeViewModel>(AddressSelected);

        }

        private Command _hospitalMapTabCommand;
        public ICommand HospitalMapTabCommand
        {
            get
            {
                return _hospitalMapTabCommand ??
                    (_hospitalMapTabCommand = new Command(OnHospitalMapTabCommand));
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
                    (_hospitalListTabCommand = new Command(OnHospitalListTabCommand));
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
                    (_newHospitalSelectedCommand = new Command(OnNewHospitalSelectedCommand));
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

        async void RefreshHospitalList(string search)
        {
            Hospitals.Clear();
            await ExecuteOperationAsync(async cancelToken =>
            {

                var hospitals = await HospitalRequestService.GetHospitalsAsync(string.Empty, cancelToken);
                Hospitals.AddRange(hospitals.Where(o => o.Name.StartsWith(search, StringComparison.CurrentCultureIgnoreCase)));

            });
        }

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

        private void PinManagement(string hospitalAddress, string hospitalName, double lat, double lng)
        {
            Pin pin = new Pin() { Address = hospitalAddress, Label = hospitalName, Position = new Xamarin.Forms.GoogleMaps.Position(lat, lng) };
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
