using Aecc.Models;
using AeccApp.Core.Models;
using System;
using System.Collections.ObjectModel;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.GoogleMaps;
using System.Threading.Tasks;

namespace AeccApp.Core.ViewModels
{
    class AllYourRequestsListViewModel : ViewModelBase
    {


        #region Constructor and initialize
        public AllYourRequestsListViewModel()
        {

        }

        public override Task ActivateAsync()
        {
            //TODO LOAD HOME AND HOSPITAL REQUESTS
            return Task.CompletedTask;
        }

        public override Task InitializeAsync(object navigationData)
        {
            if (HomeRequestsList.Count == 0)
            {
                IsHomeRequestsListEmpty = true;
            }
            if (HospitalRequestsList.Count == 0)
            {
                IsHospitalRequestsListEmpty = true;
            }

            return Task.CompletedTask;
        }
        #endregion

        #region Commands


        private Command _hospitalTabCommand;
        public ICommand HospitalTabCommand
        {
            get
            {
                return _hospitalTabCommand ??
                    (_hospitalTabCommand = new Command(OnHospitalTabCommand, (o) => !IsBusy));
            }
        }

        void OnHospitalTabCommand(object obj)
        {
            SwitchHomeAndHospitalList = true;
            RequestModel mockRequest = new RequestModel(new RequestType() { Name = "tipo" }, "location", "fecha", "hora", "comentarios", new AddressModel("Hospital rey Juan Carlos", "Fake street", "Madrid", "123", "1a", "", new Position(0, 0)));
            HospitalRequestsList.Add(mockRequest);


        }
        private Command _homeTabCommand;
        public ICommand HomeTabCommand
        {
            get
            {
                return _homeTabCommand ??
                    (_homeTabCommand = new Command(OnHomeTabCommand, (o) => !IsBusy));
            }
        }

        void OnHomeTabCommand(object obj)
        {
            SwitchHomeAndHospitalList = false;

            RequestModel mockRequest = new RequestModel(new RequestType() { Name = "tipo" }, "location", "fecha", "hora", "comentarios", new AddressModel("Mi casa", "Fake street", "Madrid", "123", "1a", "", new Position(0, 0)));
            HomeRequestsList.Add(mockRequest);
        }

     

        private Command _applyFiltersCommand;
        public ICommand ApplyFiltersCommand
        {
            get
            {
                return _applyFiltersCommand ??
                    (_applyFiltersCommand = new Command(OnApplyFiltersCommand, (o) => !IsBusy));
            }
        }

        void OnApplyFiltersCommand(object obj)
        {
            //TODO Apply home requests filters
            //TimeToFilterWith
            //DateToFilterWith
        }

        private Command _newRequestCommand;
        public ICommand NewRequestCommand
        {
            get
            {
                return _newRequestCommand ??
                    (_newRequestCommand = new Command(OnNewRequestCommand, (o) => !IsBusy));
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



        private ObservableCollection<RequestModel> _homeRequestsList = new ObservableCollection<RequestModel>();

        public ObservableCollection<RequestModel> HomeRequestsList
        {
            get { return _homeRequestsList; }
            set { Set(ref _homeRequestsList, value); }
        }
        private ObservableCollection<RequestModel> _hospitalRequestsList = new ObservableCollection<RequestModel>();

        public ObservableCollection<RequestModel> HospitalRequestsList
        {
            get { return _hospitalRequestsList; }
            set { Set(ref _hospitalRequestsList, value); }
        }


        private DateTime _dateToFilterWith = DateTime.Now;

        public DateTime DateToFilterWith
        {
            get { return _dateToFilterWith; }
            set { Set(ref _dateToFilterWith, value); }
        }


        private TimeSpan _timeToFilterWith = new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second);

        public TimeSpan TimeToFilterWith
        {
            get { return _timeToFilterWith; }
            set { Set(ref _timeToFilterWith, value); }
        }


        private bool _switchHomeAndHospitalList;

        public bool SwitchHomeAndHospitalList
        {
            get { return _switchHomeAndHospitalList; }
            set { Set(ref _switchHomeAndHospitalList, value); }
        }

  

        #endregion

        #region Methods
      
        #endregion

    }
}
