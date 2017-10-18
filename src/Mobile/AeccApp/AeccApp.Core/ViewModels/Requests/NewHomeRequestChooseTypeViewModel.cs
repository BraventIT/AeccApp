using AeccApp.Core.Models;
using AeccApp.Core.Models.Requests;
using AeccApp.Core.Services;
using System;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace AeccApp.Core.ViewModels
{
    public class NewHomeRequestChooseTypeViewModel : ViewModelBase
    {
        private readonly IGoogleMapsPlaceService _googleMapsPlaceService;
        public NewHomeRequestChooseTypeViewModel()
        {
            _googleMapsPlaceService = ServiceLocator.GoogleMapsPlaceService;
        }


        #region Commands

        public override Task InitializeAsync(object navigationData)
        {

            MyAddress = navigationData as AddressModel;

            return ExecuteOperationAsync(async () =>
            {
                GooglePlacesDetailModel places = await _googleMapsPlaceService.GetPlaceDetailAsync(MyAddress.PlaceId);
                AddCoordinatesToAddress(places);             
            });
        }

        private Command _requestCompanionForHomeCommand;
        public ICommand RequestCompanionForHomeCommand
        {
            get
            {
                return _requestCompanionForHomeCommand ??
                    (_requestCompanionForHomeCommand = new Command(OnRequestCompanionForHomeCommand, (o) => !IsBusy));
            }
        }

        async public void OnRequestCompanionForHomeCommand(object obj)
        {
            //Acompañamiento en el domicilio
            await NavigationService.NavigateToAsync<CompletingRequestViewModel>(MyAddress);

        }

        private Command _requestSupportOnHomeManagementsCommand;
        public ICommand RequestSupportOnHomeManagementsCommand
        {
            get
            {
                return _requestSupportOnHomeManagementsCommand ??
                    (_requestSupportOnHomeManagementsCommand = new Command(OnRequestSupportOnHomeManagementsCommand, (o) => !IsBusy));
            }
        }

        async public void OnRequestSupportOnHomeManagementsCommand(object obj)
        {
            await NavigationService.NavigateToAsync<CompletingRequestViewModel>(MyAddress);
            //Apoyo en gestiones en el domicilio
        }


        private Command _requestTalkToAeccCommand;
        public ICommand RequestTalkToAeccCommand
        {
            get
            {
                return _requestTalkToAeccCommand ??
                    (_requestTalkToAeccCommand = new Command(OnRequestTalkToAeccCommand, (o) => !IsBusy));
            }
        }

        public void OnRequestTalkToAeccCommand(object obj)
        {
            //Llamada al telefono de infocancer de AECC, OPCIONES 3 Y 4
            Device.OpenUri(new Uri("tel://900100036"));

        }


        #endregion


        #region Properties
    
        private AddressModel _myAddress;
        public AddressModel MyAddress
        {
            get { return _myAddress; }
            set { Set(ref _myAddress, value); }
        }
        #endregion


        #region Methods
        public void AddCoordinatesToAddress(GooglePlacesDetailModel googlePlacesDetailModel)
        {
            Position addressCoordinates = new Position(googlePlacesDetailModel.Result.Geometry.Location.Lat, googlePlacesDetailModel.Result.Geometry.Location.Lng);
            MyAddress.Coordinates = addressCoordinates;
            int n;
            bool thereIsNaturalNumberInput = int.TryParse(googlePlacesDetailModel.Result.AddressComponents[0].LongName, out n);

            if (thereIsNaturalNumberInput)
            {
                MyAddress.Number = googlePlacesDetailModel.Result.AddressComponents[0].LongName;
                MyAddress.Province = googlePlacesDetailModel.Result.AddressComponents[2].LongName;
            }
            else
            {
                MyAddress.Province = googlePlacesDetailModel.Result.AddressComponents[2].LongName;
            }
            

        }
            #endregion

        }
}
