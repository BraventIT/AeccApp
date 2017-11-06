using AeccApi.Models;
using AeccApp.Core.Models;
using AeccApp.Core.Services;
using AeccApp.Core.ViewModels.Popups;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace AeccApp.Core.ViewModels
{
    public class HospitalRequestChooseTypeViewModel : ViewModelBase
    {
        private IGoogleMapsService GoogleMapsService { get; } = ServiceLocator.GoogleMapsService;
        private IAddressesDataService HomeAddressesDataService { get; } = ServiceLocator.HomeAddressesDataService;
        private IHospitalRequestService HospitalRequestService { get; } = ServiceLocator.HospitalRequestService;

        public HospitalRequestChooseTypeViewModel()
        {
            RequestHospitalAskForRoomPopupVM = new RequestHospitalAskForRoomPopupViewModel();
            RequestHospitalAskForRoomPopupVM.ContinueWithRequest += OnContinueWithRequest;
        }

        public override Task InitializeAsync(object navigationData)
        {
            HospitalAddress = navigationData as AddressModel;
            if (HospitalAddress == null)
            {
                throw new ArgumentNullException("AddressModel object required");
            }

            Request.RequestAddress = HospitalAddress;

            return Task.CompletedTask;
        }

        public override Task ActivateAsync()
        {
            return ExecuteOperationAsync(async cancelToken =>
            {
                ProvinceHospitals = await HospitalRequestService.GetHospitalsAsync(HospitalAddress.Province, cancelToken);
                if (ProvinceHospitals != null && ProvinceHospitals.Any())
                {
                    RequestTypes = await HospitalRequestService.GetRequestTypesAsync(cancelToken);
                }
                else
                {
                    ProvinceHasNotRequestAvailable = true;
                }
            });
        }

        #region Properties

        public RequestHospitalAskForRoomPopupViewModel RequestHospitalAskForRoomPopupVM { get; private set; }


        private IEnumerable<Hospital> _provinceHospitals;
        public IEnumerable<Hospital> ProvinceHospitals
        {
            get { return _provinceHospitals; }
            set { Set(ref _provinceHospitals, value); }
        }

        private IEnumerable<RequestType> _requestTypes = new ObservableCollection<RequestType>();
        public IEnumerable<RequestType> RequestTypes
        {
            get { return _requestTypes; }
            set { Set(ref _requestTypes, value); }
        }

        private bool _provinceHasNotRequestAvailable;

        public bool ProvinceHasNotRequestAvailable
        {
            get { return _provinceHasNotRequestAvailable; }
            set { Set(ref _provinceHasNotRequestAvailable, value); }
        }


        private AddressModel _hospitalAddress;
        public AddressModel HospitalAddress
        {
            get { return _hospitalAddress; }
            set { Set(ref _hospitalAddress, value); }
        }

        private RequestModel _request = new RequestModel();

        public RequestModel Request
        {
            get { return _request; }
            set { _request = value; }
        }


        #endregion

        #region Commands

        private async void OnContinueWithRequest(object sender, EventArgs e)
        {
            Request.RequestAddress.HospitalHall = RequestHospitalAskForRoomPopupVM.Hall;
            Request.RequestAddress.HospitalRoom = RequestHospitalAskForRoomPopupVM.Room;
            await NavigationService.HidePopupAsync();
            await NavigationService.NavigateToAsync<CompletingHospitalRequestViewModel>(Request);
        }


        private Command _requestTypeCommand;
        public ICommand RequestTypeCommand
        {
            get
            {
                return _requestTypeCommand ??
                    (_requestTypeCommand = new Command(OnRequestTypeCommand, (o) => !IsBusy));
            }
        }

        async public void OnRequestTypeCommand(object obj)
        {
            var requestType = obj as RequestType;
            Request.RequestAddress = HospitalAddress;
            Request.RequestType = requestType;
            await NavigationService.ShowPopupAsync(RequestHospitalAskForRoomPopupVM);

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
