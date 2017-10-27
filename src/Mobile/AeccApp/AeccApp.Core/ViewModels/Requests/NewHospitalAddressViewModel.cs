using AeccApp.Core.Extensions;
using AeccApp.Core.Messages;
using AeccApp.Core.Services;
using Plugin.Geolocator.Abstractions;
using System.Windows.Input;
using Xamarin.Forms;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using Xamarin.Forms.GoogleMaps;
using System.Linq;
using AeccApi.Models;

namespace AeccApp.Core.ViewModels
{
    public class NewHospitalAddressViewModel : ViewModelBase
    {
        private IGeolocator GeolocatorService { get; } = ServiceLocator.GeolocatorService;
        private IHospitalRequestService HospitalRequestService { get; } = ServiceLocator.HospitalRequestService;
        private IMapPinsDataService MapPinsDataService { get; } = ServiceLocator.MapPinsDataService;


        public async override Task ActivateAsync()
        {


            Xamarin.Forms.GoogleMaps.Position position = new Xamarin.Forms.GoogleMaps.Position();
            if (GeolocatorService.IsGeolocationEnabled)
            {
                position = await GeolocatorService.GetCurrentLocationAsync();
                MessagingCenter.Send(new GeolocatorMessages(GeolocatorEnum.Refresh), string.Empty, position);

                var addresses = await GeolocatorService.GetAddressesForPositionAsync(new Plugin.Geolocator.Abstractions.Position(position.Latitude,position.Longitude), GlobalSetting.Instance.GooglePlacesApiKey);           
                var address = addresses.FirstOrDefault();
                var Hospitals = await HospitalRequestService.GetHospitalsAsync(address.SubAdminArea) ;
                foreach (var item in Hospitals)
                {

                }
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
            MessagingCenter.Send(new GeolocatorMessages(GeolocatorEnum.Refresh),string.Empty, position) ;
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





    }
}
