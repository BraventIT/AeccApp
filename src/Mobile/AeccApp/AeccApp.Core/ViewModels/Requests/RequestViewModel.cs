using AeccApp.Core.Extensions;
using AeccApp.Core.Models;
using AeccApp.Core.Services;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace AeccApp.Core.ViewModels
{
    public class RequestsViewModel : ViewModelBase
    {
        private IHomeRequestsDataService HomeRequestsDataService { get; } = ServiceLocator.HomeRequestsDataService;
        private IHospitalRequestDataService HospitalRequestDataService { get; } = ServiceLocator.HospitalRequestDataService;

        #region Constructor and initialize
        public RequestsViewModel()
        {
            FirstTimeLandingPageVisible = Settings.FirstRequestLandingPageSeen;
            _homeRequestsList = new List<RequestModel>();
            _hospitalRequestsList = new List<RequestModel>();
        }

        public override async Task ActivateAsync()
        {
            FirstTimeLandingPageVisible = Settings.FirstRequestLandingPageSeen;

            if (!FirstTimeLandingPageVisible)
                await ExecuteOperationAsync(async () =>
                {
                    var homeRequests = await HomeRequestsDataService.GetListAsync();
                    HomeRequestsList.SyncExact(homeRequests);
                    var HospitalRequest = await HospitalRequestDataService.GetListAsync();
                    HospitalRequestsList.AddRange(HospitalRequest);

                    if (!HomeRequestsList.Any())
                    {
                        IsHomeRequestsListEmpty = true;
                    }
                    if (!HospitalRequestsList.Any())
                    {
                        IsHospitalRequestsListEmpty = true;
                    }
                });
        }

        #endregion

        #region Properties
        private bool _firstTimeLandingPageVisible;

        public bool FirstTimeLandingPageVisible
        {
            get { return _firstTimeLandingPageVisible; }
            set { Set(ref _firstTimeLandingPageVisible, value); }
        }

        private bool _isHomeRequestsListEmpty;

        public bool IsHomeRequestsListEmpty
        {
            get { return _isHomeRequestsListEmpty; }
            set { Set(ref _isHomeRequestsListEmpty, value); }
        }

        private bool _isHospitalRequestsListEmpty;

        public bool IsHospitalRequestsListEmpty
        {
            get { return _isHospitalRequestsListEmpty; }
            set { Set(ref _isHospitalRequestsListEmpty, value); }
        }

        private List<RequestModel> _homeRequestsList;

        public List<RequestModel> HomeRequestsList
        {
            get { return _homeRequestsList; }
        }
        private List<RequestModel> _hospitalRequestsList;

        public List<RequestModel> HospitalRequestsList
        {
            get { return _hospitalRequestsList; }
        }

        private bool _switchHomeAndHospitalList;

        public bool SwitchHomeAndHospitalList
        {
            get { return _switchHomeAndHospitalList; }
            set { Set(ref _switchHomeAndHospitalList, value); }
        }
        #endregion

        #region Commands
        private Command _continueWithRequest;
        public ICommand ContinueWithRequest
        {
            get
            {
                return _continueWithRequest ??
                    (_continueWithRequest = new Command(OnContinueWithRequest));
            }
        }

     
        void OnContinueWithRequest(object obj)
        {
            Settings.FirstRequestLandingPageSeen = false;
            FirstTimeLandingPageVisible = false;
        }

        private Command _newRequestCommand;
        public ICommand NewRequestCommand
        {
            get
            {
                return _newRequestCommand ??
                    (_newRequestCommand = new Command(OnNewRequestCommand));
            }
        }

        /// <summary>
        /// Navigates to NewRequestSelectAddressView
        /// </summary>
        /// <param name="obj"></param>
        async void OnNewRequestCommand(object obj)
        {
            await NavigationService.NavigateToAsync<NewRequestSelectAddressViewModel>();
        }

        private Command _hospitalTabCommand;
        public ICommand HospitalTabCommand
        {
            get
            {
                return _hospitalTabCommand ??
                    (_hospitalTabCommand = new Command(o => SwitchHomeAndHospitalList = true));
            }
        }

        private Command _homeTabCommand;
        public ICommand HomeTabCommand
        {
            get
            {
                return _homeTabCommand ??
                    (_homeTabCommand = new Command(o=> SwitchHomeAndHospitalList = false));
            }
        }

        #endregion





    }
}
