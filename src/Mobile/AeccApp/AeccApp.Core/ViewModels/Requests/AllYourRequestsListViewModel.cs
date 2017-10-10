using AeccApp.Core.Models.Requests;
using AeccApp.Internationalization.Properties;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Resources;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace AeccApp.Core.ViewModels
{
    class AllYourRequestsListViewModel : ViewModelBase
    {
      

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
            RequestModel mockRequest = new RequestModel("tipo", "location", "fecha", "hora", "comentarios", new AddressModel("Hospital rey Juan Carlos", "Fake street", "Madrid", "123", "1a", "",""));
            mockRequest.RequestAddress.DisplayAddress = mockRequest.RequestAddress.AddressName + "-" + mockRequest.RequestAddress.AddressStreet + "," + mockRequest.RequestAddress.AddressNumber;
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

            RequestModel mockRequest = new RequestModel("tipo", "location", "fecha", "hora", "comentarios", new AddressModel("Mi casa", "Fake street", "Madrid", "123", "1a", "",""));
            mockRequest.RequestAddress.DisplayAddress = mockRequest.RequestAddress.AddressName + "-" + mockRequest.RequestAddress.AddressStreet + "," + mockRequest.RequestAddress.AddressNumber;
            HomeRequestsList.Add(mockRequest);

        }

        private Command _openCloseFilterPopupCommand;
        public ICommand OpenCloseFilterPopupCommand
        {
            get
            {
                return _openCloseFilterPopupCommand ??
                    (_openCloseFilterPopupCommand = new Command(OnOpenCloseFilterPopup, (o) => !IsBusy));
            }
        }


        void OnOpenCloseFilterPopup(object obj)
        {
            IsFilterPopupVisible = !IsFilterPopupVisible;

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
            IsFilterPopupVisible = !IsFilterPopupVisible;
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

        private bool _isFilterPopupVisible;

        public bool IsFilterPopupVisible
        {
            get { return _isFilterPopupVisible; }
            set { Set(ref _isFilterPopupVisible, value); }
        }

        #endregion


        #region Methods
        public override bool OnBackButtonPressed()
        {
            bool returnValue = false;
            if (IsFilterPopupVisible)
            {
                IsFilterPopupVisible = false;
                returnValue =  true;
            }
            return returnValue;
        }
        #endregion




    }
}
