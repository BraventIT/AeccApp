using AeccApp.Core.Models.Requests;
using AeccApp.Core.Services;
using AeccApp.Internationalization.Properties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Resources;
using System.Text;
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
            _googleMapsPlaceService = ServiceLocator.Resolve<IGoogleMapsPlaceService>();
        }


        #region Commands

        public override Task InitializeAsync(object navigationData)
        {

            MyAddress = navigationData as AddressModel;
            MyAddress.PlaceId.ToString();
            return ExecuteOperationAsync(async () =>
            {
                GooglePlacesDetailModel places = await _googleMapsPlaceService.GetPlaceDetailAsync(MyAddress.PlaceId);
                MyAddress.Lat = places.Result.Geometry.Location.Lat;
                MyAddress.Lng = places.Result.Geometry.Location.Lng;

                int n;
                bool thereIsNaturalNumberInput = int.TryParse(places.Result.AddressComponents[0].LongName, out n);
                
                if (thereIsNaturalNumberInput)
                {
                    MyAddress.AddressNumber = places.Result.AddressComponents[0].LongName;
                    MyAddress.AddressProvince = places.Result.AddressComponents[2].LongName;
                }
                else
                {
                    MyAddress.AddressProvince = places.Result.AddressComponents[2].LongName;
                }

                
                

                int I = 0;
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
        
        private AddressModel _myAddress = new AddressModel();
        public AddressModel MyAddress
        {
            get { return _myAddress; }
            set { Set(ref _myAddress, value); }
        }
        #endregion

    }
}
