using Aecc.Models;
using AeccApp.Core.Models;
using System;
using System.Collections.ObjectModel;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.GoogleMaps;
using System.Threading.Tasks;
using AeccApp.Core.Services;
using System.Collections.Generic;

namespace AeccApp.Core.ViewModels
{
    class AllYourRequestsListViewModel : ViewModelBase
    {

        private IHomeRequestsDataService HomeRequestsDataService { get; } = ServiceLocator.HomeRequestsDataService;
        private IHospitalRequestDataService HospitalRequestDataService { get; } = ServiceLocator.HospitalRequestDataService;

        #region Constructor and initialize


        public override Task ActivateAsync()
        {
            return ExecuteOperationAsync(async () =>
            {
                HomeRequestsList = new List<RequestModel>();
                HospitalRequestsList = new List<RequestModel>();

                var HomeRequests = await HomeRequestsDataService.GetListAsync();
                HomeRequestsList.AddRange(HomeRequests);
                var HospitalRequest = await HospitalRequestDataService.GetListAsync();
                HospitalRequestsList.AddRange(HospitalRequest);

                if (HomeRequestsList.Count == 0)
                {
                    IsHomeRequestsListEmpty = true;
                }
                if (HospitalRequestsList.Count == 0)
                {
                    IsHospitalRequestsListEmpty = true;
                }

            });


        }

     
        #endregion

        #region Commands


        private Command _hospitalTabCommand;
        public ICommand HospitalTabCommand
        {
            get
            {
                return _hospitalTabCommand ??
                    (_hospitalTabCommand = new Command(OnHospitalTabCommand, o => !IsBusy));
            }
        }

        void OnHospitalTabCommand(object obj)
        {
            SwitchHomeAndHospitalList = true;
        }
        private Command _homeTabCommand;
        public ICommand HomeTabCommand
        {
            get
            {
                return _homeTabCommand ??
                    (_homeTabCommand = new Command(OnHomeTabCommand, o => !IsBusy));
            }
        }

        void OnHomeTabCommand(object obj)
        {
            SwitchHomeAndHospitalList = false;
        }
 

        private Command _newRequestCommand;
        public ICommand NewRequestCommand
        {
            get
            {
                return _newRequestCommand ??
                    (_newRequestCommand = new Command(OnNewRequestCommand, o => !IsBusy));
            }
        }

        async void OnNewRequestCommand(object obj)
        {
            await NavigationService.NavigateToAsync<NewRequestSelectAddressViewModel>();


        }

        #endregion

        #region Properties
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
            set { Set(ref _homeRequestsList, value); }
        }
        private List<RequestModel> _hospitalRequestsList;

        public List<RequestModel> HospitalRequestsList
        {
            get { return _hospitalRequestsList; }
            set { Set(ref _hospitalRequestsList, value); }
        }


        private bool _switchHomeAndHospitalList;

        public bool SwitchHomeAndHospitalList
        {
            get { return _switchHomeAndHospitalList; }
            set { Set(ref _switchHomeAndHospitalList, value); }
        }

  

        #endregion

   

    }
}
