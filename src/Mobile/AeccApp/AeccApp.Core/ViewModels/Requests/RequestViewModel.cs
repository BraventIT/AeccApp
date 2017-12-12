using AeccApp.Core.Extensions;
using AeccApp.Core.Messages;
using AeccApp.Core.Models;
using AeccApp.Core.Services;
using System.Collections.ObjectModel;
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
            _homeRequestsList = new ObservableCollection<RequestModel>();
            _hospitalRequestsList = new ObservableCollection<RequestModel>();
        }

        public override async Task ActivateAsync()
        {
            if (FirstTimeLandingPageVisible)
                MessagingCenter.Send(new ToolbarMessage(this["NewRequestToolbarTitle"]), string.Empty);
            else {
                MessagingCenter.Send(new ToolbarMessage(this["YourRequestsToolbarTitle"]), string.Empty);
                await RefreshRequestAsync(SwitchHomeAndHospitalList);
            }
        }

        #endregion

        #region Properties
        private bool _firstTimeLandingPageVisible;
        public bool FirstTimeLandingPageVisible
        {
            get { return Settings.FirstRequestLandingPageSeen; }
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

        private ObservableCollection<RequestModel> _homeRequestsList;
        public ObservableCollection<RequestModel> HomeRequestsList
        {
            get { return _homeRequestsList; }
        }

        private ObservableCollection<RequestModel> _hospitalRequestsList;
        public ObservableCollection<RequestModel> HospitalRequestsList
        {
            get { return _hospitalRequestsList; }
        }

        private bool _switchHomeAndHospitalList;
        public bool SwitchHomeAndHospitalList
        {
            get { return _switchHomeAndHospitalList; }
            set
            {
                if (Set(ref _switchHomeAndHospitalList, value))
                {
                    RefreshRequestAsync(_switchHomeAndHospitalList);
                }
            }
        }
        #endregion

        #region Commands
        private Command _continueWithRequest;
        public ICommand ContinueWithRequest
        {
            get
            {
                return _continueWithRequest ??
                    (_continueWithRequest = new Command(o=> OnContinueWithRequestAsync()));
            }
        }

        async Task OnContinueWithRequestAsync()
        {
            Settings.FirstRequestLandingPageSeen = false;
            NotifyPropertyChanged(nameof(FirstTimeLandingPageVisible));

            await RefreshRequestAsync(SwitchHomeAndHospitalList);
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

        private Command _atHomeCommand;
        public ICommand AtHomeCommand
        {
            get
            {
                return _atHomeCommand ??
                    (_atHomeCommand = new Command(OnAtHomeCommand));
            }
        }

        async void OnAtHomeCommand(object obj)
        {
            await NavigationService.NavigateToAsync<HomeAddressesListViewModel>();
        }


        private Command _atHospitalCommand;
        public ICommand AtHospitalCommand
        {
            get
            {
                return _atHospitalCommand ??
                    (_atHospitalCommand = new Command(OnAtHospitalCommand));
            }
        }

        async void OnAtHospitalCommand(object obj)
        {
            await NavigationService.NavigateToAsync<HospitalAddressesListViewModel>();
        }

        #endregion

        #region Private Methods
        private Task RefreshRequestAsync(bool switchHomeAndHospitalList)
        {
            return ExecuteOperationAsync(async () =>
            {
                if (switchHomeAndHospitalList)
                {
                    var hospitalRequest = await HospitalRequestDataService.GetListAsync();
                    HospitalRequestsList.SyncExact(hospitalRequest);
                    IsHospitalRequestsListEmpty = !HospitalRequestsList.Any();
                }
                else
                {
                    var homeRequests = await HomeRequestsDataService.GetListAsync();
                    HomeRequestsList.SyncExact(homeRequests);
                    IsHomeRequestsListEmpty = !HomeRequestsList.Any();
                }
            });
        }

        #endregion
    }
}
