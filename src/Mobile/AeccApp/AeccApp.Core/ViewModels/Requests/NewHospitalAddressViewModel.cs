using AeccApp.Core.Extensions;
using AeccApp.Core.Messages;
using AeccApp.Core.Services;
using Plugin.Geolocator.Abstractions;
using System.Windows.Input;
using Xamarin.Forms;
using System.Threading.Tasks;

namespace AeccApp.Core.ViewModels
{
    public class NewHospitalAddressViewModel : ViewModelBase
    {
        private IGeolocator GeolocatorService { get; } = ServiceLocator.GeolocatorService;

        public async override Task ActivateAsync()
        {
            Xamarin.Forms.GoogleMaps.Position position = new Xamarin.Forms.GoogleMaps.Position();
            if (GeolocatorService.IsGeolocationEnabled)
            {
                position = await GeolocatorService.GetCurrentLocationAsync();
                MessagingCenter.Send(new GeolocatorMessages(GeolocatorEnum.Refresh), string.Empty, position);
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


        private bool _switchBetweenAndHospitalList;

        public bool SwitchBetweenAndHospitalList
        {
            get { return _switchBetweenAndHospitalList; }
            set { Set(ref _switchBetweenAndHospitalList, value); }
        }

  
        #endregion





    }
}
