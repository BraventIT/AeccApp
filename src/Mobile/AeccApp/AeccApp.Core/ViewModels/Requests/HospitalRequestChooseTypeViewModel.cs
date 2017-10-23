using AeccApp.Core.Models;
using AeccApp.Core.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace AeccApp.Core.ViewModels
{
    public class HospitalRequestChooseTypeViewModel : ViewModelBase
    {
        private IGoogleMapsPlaceService GoogleMapsPlaceService { get; } = ServiceLocator.GoogleMapsPlaceService;
        private IHomeAddressesDataService HomeAddressesDataService { get; } = ServiceLocator.HomeAddressesDataService;

        #region Properties

        private AddressModel _myAddress;
        public AddressModel MyAddress
        {
            get { return _myAddress; }
            set { Set(ref _myAddress, value); }
        }
        #endregion



        public override Task InitializeAsync(object navigationData)
        {
            MyAddress = navigationData as AddressModel;
            if (MyAddress == null)
                throw new ArgumentNullException("AddressModel object required");

            return Task.CompletedTask;
        }

        public override Task ActivateAsync()
        {
            return ExecuteOperationAsync(async () =>
            {
                if (MyAddress.Coordinates == null)
                {
                    MyAddress = await GoogleMapsPlaceService.FillPlaceDetailAsync(MyAddress);
                }
                if (MyAddress.WillBeSaved)
                {
                    MyAddress.WillBeSaved = false;
                    await HomeAddressesDataService.AddOrUpdateAddressAsync(MyAddress);
                }
            });
        }

        #region Commands

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
            await NavigationService.NavigateToAsync<CompletingHomeRequestViewModel>(MyAddress);
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
            await NavigationService.NavigateToAsync<CompletingHomeRequestViewModel>(MyAddress);
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

    }
}
