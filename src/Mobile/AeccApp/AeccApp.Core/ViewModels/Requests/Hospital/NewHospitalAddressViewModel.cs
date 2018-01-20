using Aecc.Extensions;
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
                            PinManagement(hospital.Street, hospital.Name,hospital.ID ,location.Latitude, location.Longitude);
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

        private bool _hospitalsListIsEmpty;
        public bool HospitalsListIsEmpty
        {
            get { return _hospitalsListIsEmpty; }
            set { Set(ref _hospitalsListIsEmpty, value); }
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
            HospitalsListIsEmpty = false;
            Hospitals.Clear();
            await ExecuteOperationAsync(cancelToken =>
            {
                if (!Hospitals.Any())
                {
                    var hospitals = GlobalSetting.Instance.Hospitals;
                    Hospitals.AddRange(hospitals);
                }
                return Task.CompletedTask;
            });

        }


        private Command _addressChangedCommand;
        public ICommand AddressChangedCommand
        {
            get
            {
                return _addressChangedCommand ??
                   (_addressChangedCommand = new Command(async o => await OnAddressChanged(o)));
            }
        }

        private async Task OnAddressChanged(object obj)
        {
            HospitalsListIsEmpty = false;
            var hospitals = GlobalSetting.Instance.Hospitals;
            string result = string.Empty;
            if (obj is string)
            {
                result = (string)obj;
            }

            if (result.Length > 3)
            {

                if (string.IsNullOrWhiteSpace(result))
                {
                    IsSearchIconVisible = false;
                    Hospitals.Clear();
                    await ExecuteOperationAsync(() =>
                    {
                        if (!Hospitals.Any())
                        {
                            Hospitals.AddRange(hospitals);
                        }
                        return Task.CompletedTask;
                    });
                }
                else
                {
                    RefreshHospitalList(result);
                    IsSearchIconVisible = true;
                }
            }
            if (result.Length < 1)
            {
                IsSearchIconVisible = false;             
                Hospitals.Clear();
                Hospitals.AddRange(hospitals);
            }
            else
            {
                IsSearchIconVisible = true;
            }
        }

        private Command _infoWindowClickedCommand;
        public ICommand InfoWindowClickedCommand
        {
            get
            {
                return _infoWindowClickedCommand ??
                    (_infoWindowClickedCommand = new Command(async o => await OnInfoWindowClickedCommand(o)));
            }
        }

        async Task OnInfoWindowClickedCommand(object obj)
        {
            var pinClicked = obj as Pin;
            AddressSelected.HospitalID = (int)pinClicked.Tag;
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
            HospitalsListIsEmpty = false;
            SwitchBetweenAndHospitalList = false;
            await ExecuteOperationAsync(async cancelToken =>
            {
                if (!Hospitals.Any())
                {
                    var hospitals = await HospitalRequestService.GetHospitalsAsync(string.Empty, cancelToken);
                    GlobalSetting.Instance.Hospitals = new ObservableCollection<Hospital>();
                    GlobalSetting.Instance.Hospitals.AddRange(hospitals);
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
                AddressSelected.HospitalID = hospitalSelected.ID;
                AddressSelected.Street = hospitalSelected.Street;
                AddressSelected.Name = hospitalSelected.Name;
                AddressSelected.Coordinates = await GetLocationForHospitalAsync(hospitalSelected, cancelToken);

                await NavigationService.NavigateToAsync<HospitalRequestChooseTypeViewModel>(AddressSelected);
            });
        }

        #endregion

        #region Methods

        void RefreshHospitalList(string search)
        {
            HospitalsListIsEmpty = false;
            Hospitals.Clear();
            var hospitals = GlobalSetting.Instance.Hospitals;
            Hospitals.AddRange(hospitals.Where(o => o.Name.Contains(search, StringComparison.CurrentCultureIgnoreCase)));
            if (Hospitals.Count == 0)
            {
                Hospitals.AddRange(hospitals.Where(o => o.Province.Contains(search, StringComparison.CurrentCultureIgnoreCase)));
            }
            if (Hospitals.Count == 0)
            {
                HospitalsListIsEmpty = true;
            }
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
                        await MapPositionsDataService.InsertOrUpdateAsync(hospitalAddress, location);
                }
            }
            return location;
        }

        private void PinManagement(string hospitalAddress, string hospitalName,int hospitalID, double lat, double lng)
        {
            Pin pin = new Pin() {Tag=hospitalID,  Address = hospitalAddress, Label = hospitalName, Position = new Xamarin.Forms.GoogleMaps.Position(lat, lng) };
            switch (Device.RuntimePlatform)
            {
                case Device.Android:
                    pin.Icon = BitmapDescriptorFactory.FromBundle($"map_location_pin_green.png");
                    break;
                case Device.iOS:
                    pin.Icon = BitmapDescriptorFactory.FromBundle($"map_location_pin_green.png");
                    break;
                case Device.UWP:
                    pin.Icon = BitmapDescriptorFactory.FromBundle($"Assets/map_location_pin_green.png");
                    break;
                default:
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
