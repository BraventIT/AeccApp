using Aecc.Models;
using AeccApp.Core.Extensions;
using AeccApp.Core.Models;
using AeccApp.Core.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace AeccApp.Core.ViewModels
{
    public class HomeRequestChooseTypeViewModel : ViewModelBase
    {
        private IGoogleMapsService GoogleMapsService { get; } = ServiceLocator.GoogleMapsService;
        private IAddressesDataService HomeAddressesDataService { get; } = ServiceLocator.HomeAddressesDataService;
        private IHomeRequestService HomeRequestService { get; } = ServiceLocator.HomeRequestService;
        private IHomeRequestsTypesDataService HomeRequestsTypesDataService { get; } = ServiceLocator.HomeRequestsTypesDataService;

        #region Properties

        private RequestModel _request = new RequestModel();

        public RequestModel Request
        {
            get { return _request; }
            set { Set(ref _request, value); }
        }


        private AddressModel _myAddress;
        public AddressModel MyAddress
        {
            get { return _myAddress; }
            set { Set(ref _myAddress, value); }
        }

        private IEnumerable<Coordinator> _provinceCoordinators;
        public IEnumerable<Coordinator> ProvinceCoordinators
        {
            get { return _provinceCoordinators; }
            set { Set(ref _provinceCoordinators, value); }
        }

        private ObservableCollection<RequestType> _requestTypes;
        public ObservableCollection<RequestType> RequestTypes
        {
            get { return _requestTypes; }
        }

        private bool _provinceHasNotRequestAvailable;

        public bool ProvinceHasNotRequestAvailable
        {
            get { return _provinceHasNotRequestAvailable; }
            set { Set(ref _provinceHasNotRequestAvailable, value); }
        }


        #endregion

        public HomeRequestChooseTypeViewModel()
        {
            _requestTypes = new ObservableCollection<RequestType>();
        }

        public override Task InitializeAsync(object navigationData)
        {
            MyAddress = navigationData as AddressModel;
            if (MyAddress == null)
                throw new ArgumentNullException("AddressModel object required");


            return Task.CompletedTask;
        }

        public override async Task ActivateAsync()
        {
            await ExecuteOperationAsync(async cancelToken =>
            {
                await FillTypesAsync();

                if (MyAddress.Coordinates.Latitude == 0)
                {
                    MyAddress = await GoogleMapsService.FillPlaceDetailAsync(MyAddress, cancelToken);
                }

                ProvinceCoordinators = await HomeRequestService.GetCoordinatorsAsync(MyAddress.Province, cancelToken);
                if (ProvinceCoordinators == null || !ProvinceCoordinators.Any())
                {
                    ProvinceHasNotRequestAvailable = true;
                }

                await ExecuteOperationQuietlyAsync(async cancTok =>
                {
                    if (MyAddress.WillBeSaved)
                    {
                        MyAddress.WillBeSaved = false;
                        await HomeAddressesDataService.InsertOrUpdateAsync(MyAddress);
                    }
                    if (!ProvinceHasNotRequestAvailable)
                    {
                        await TryToUpdateTypesAsync(cancTok);
                    }
                });
            });
        }

        #region Commands

        private Command _requestTypeCommand;
        public ICommand RequestTypeCommand
        {
            get
            {
                return _requestTypeCommand ??
                    (_requestTypeCommand = new Command(OnRequestTypeCommand, o => !IsBusy));
            }
        }

        async public void OnRequestTypeCommand(object obj)
        {
            var requestType = obj as RequestType;
            Request.RequestType = requestType;
            Request.RequestAddress = MyAddress;
            await NavigationService.NavigateToAsync<CompletingHomeRequestViewModel>(Request);

        }

        private Command _requestTalkToAeccCommand;
        public ICommand RequestTalkToAeccCommand
        {
            get
            {
                return _requestTalkToAeccCommand ??
                    (_requestTalkToAeccCommand = new Command(OnRequestTalkToAeccCommand, o => !IsBusy));
            }
        }

        public void OnRequestTalkToAeccCommand(object obj)
        {
            //Llamada al telefono de infocancer de AECC, OPCIONES 3 Y 4
            Device.OpenUri(new Uri("tel://900100036"));

        }

        #endregion


        #region Methods
        private async Task FillTypesAsync()
        {
            var types = await HomeRequestsTypesDataService.GetListAsync();
            if (types.Any())
            {
                types.Reverse();
                RequestTypes.SyncExact(types);
            }
        }

        private async Task TryToUpdateTypesAsync(CancellationToken cancelToken)
        {
            
            var types = await HomeRequestService.GetRequestTypesAsync(cancelToken);

            foreach (var typesData in types)
            {
                    await HomeRequestsTypesDataService.InsertOrUpdateAsync(typesData);
            }
            RequestTypes.SyncExact(types);
        }

        #endregion

    }
}
