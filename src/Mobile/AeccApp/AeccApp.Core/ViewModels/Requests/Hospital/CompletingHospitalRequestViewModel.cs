using AeccApp.Core.Messages;
using AeccApp.Core.Models;
using AeccApp.Core.Services;
using AeccApp.Core.ViewModels.Popups;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace AeccApp.Core.ViewModels
{
    public class CompletingHospitalRequestViewModel : ViewModelBase
    {
        private IAddressesDataService HomeAddressesDataService { get; } = ServiceLocator.HomeAddressesDataService;


        #region Constructor and initialization

        public CompletingHospitalRequestViewModel()
        {
            RequestDateAndTimePopupVM = new RequestDateAndTimePopupViewModel();
            RequestConfirmationPopupVM = new RequestConfirmationPopupViewModel();
            RequestSentPopupVM = new RequestSentPopupViewModel();
        }

        public override Task InitializeAsync(object navigationData)
        {
            CurrentRequest = navigationData as RequestModel;
            CurrentAddress = CurrentRequest.RequestAddress;
            InitialMapLat = CurrentAddress.Coordinates.Latitude;
            InitialMapLng = CurrentAddress.Coordinates.Longitude;
            RequestTypeHeader = CurrentRequest.RequestType.Name;
            return Task.CompletedTask;
        }
        public override Task ActivateAsync()
        {
            Xamarin.Forms.GoogleMaps.Position position = new Xamarin.Forms.GoogleMaps.Position(InitialMapLat, InitialMapLng);
            MessagingCenter.Send(new GeolocatorMessage(GeolocatorEnum.Refresh), string.Empty, position);
            RequestDateAndTimePopupVM.ApplyDateAndTime += OnApplyDateAndTimeCommand;
            RequestConfirmationPopupVM.ConfirmRequestToSend += OnSendRequestConfirmationCommand;
            return Task.CompletedTask;
        }
        public override void Deactivate()
        {
            RequestDateAndTimePopupVM.ApplyDateAndTime -= OnApplyDateAndTimeCommand;
            RequestConfirmationPopupVM.ConfirmRequestToSend -= OnSendRequestConfirmationCommand;
        }

        #endregion

        #region Commands

        private Command _addressGettingSaved;
        public ICommand AddressGettingSavedCommand
        {
            get
            {
                return _addressGettingSaved ??
                    (_addressGettingSaved = new Command(OnAddressGettingSaved, o => !IsBusy));
            }
        }

        private void OnAddressGettingSaved(object obj)
        {
            bool result = false;
            if (obj is bool)
                result = (bool)obj;

            IsAddressGettingSaved = result;
        }

        private Command _mapDetailCommand;
        public ICommand MapDetailCommand
        {
            get
            {
                return _mapDetailCommand ??
                    (_mapDetailCommand = new Command(OnMapDetailCommand, (o) => !IsBusy));
            }
        }

        private async void OnMapDetailCommand(object obj)
        {
            await NavigationService.NavigateToAsync<MapDetailViewModel>(new Xamarin.Forms.GoogleMaps.Position(InitialMapLat, InitialMapLng));
        }


        private Command _openRequestConfirmationPopupCommand;
        public ICommand OpenRequestConfirmationPopupCommand
        {
            get
            {
                return _openRequestConfirmationPopupCommand ??
                    (_openRequestConfirmationPopupCommand = new Command(OnOpenRequestConfirmationPopupCommand, (o) => !IsBusy));
            }
        }

        private async void OnOpenRequestConfirmationPopupCommand(object obj)
        {
            if (DateToApplyParsed == null)
            {
                RequestConfirmationPopupVM.DisplayDate = DateTime.Now.ToString().Remove(10);
                DateToApplyParsed = DateTime.Now.ToString().Remove(10);
            }
            else
            {
                RequestConfirmationPopupVM.DisplayDate = DateToApplyParsed;
            }

            if (TimeToApplyParsed == null)
            {
                RequestConfirmationPopupVM.DisplayTime = new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second).ToString().Remove(5);
                TimeToApplyParsed = new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second).ToString().Remove(5);
            }
            else
            {
                RequestConfirmationPopupVM.DisplayTime = TimeToApplyParsed;
            }

            await NavigationService.ShowPopupAsync(RequestConfirmationPopupVM);


        }


        private Command _openDateAndTimePopupCommand;
        public ICommand OpenDateAndTimePopupCommand
        {
            get
            {
                return _openDateAndTimePopupCommand ??
                    (_openDateAndTimePopupCommand = new Command(OnOpenDateAndTimePopupCommand, (o) => !IsBusy));
            }
        }

        private async void OnOpenDateAndTimePopupCommand(object obj)
        {
            await NavigationService.ShowPopupAsync(RequestDateAndTimePopupVM);
        }


        private async void OnApplyDateAndTimeCommand(object sender, EventArgs e)
        {
            //Applies date and time to the request:
            DateToApplyParsed = RequestDateAndTimePopupVM.DateSelected.Date.ToString().Remove(10);
            TimeToApplyParsed = RequestDateAndTimePopupVM.TimeSelected.ToString().Remove(5);
            await NavigationService.HidePopupAsync();

        }

        private Command _commentsEntryListenerCommand;
        public ICommand CommentsEntryListenerCommand
        {
            get
            {
                return _commentsEntryListenerCommand ??
                    (_commentsEntryListenerCommand = new Command(OnCommentsEntryListenerCommand, (o) => !IsBusy));
            }
        }

        private void OnCommentsEntryListenerCommand(object obj)
        {
            string result = string.Empty;
            if (obj is string)
            {
                result = (string)obj;
                if (RequestComments.Length <= 120)
                {
                    CommentsLenghtReminder = result.Length + "/120";
                }
                else
                {
                    RequestComments = RequestComments.Remove(120);
                }
            }
        }

        private async void OnSendRequestConfirmationCommand(object sender, EventArgs e)
        {
            CurrentRequest.RequestComments = RequestComments;
            CurrentRequest.RequestDate = DateToApplyParsed;
            CurrentRequest.RequestTime = TimeToApplyParsed;
            if (IsAddressGettingSaved)
            {
                CurrentRequest.RequestAddress.IsHospitalAddress = true;
                await HomeAddressesDataService.AddOrUpdateAsync(CurrentRequest.RequestAddress);
            }
            await NavigationService.HidePopupAsync();
            //TODO send request
            await NavigationService.ShowPopupAsync(RequestSentPopupVM);
        }


        #endregion

        #region Properties

        public RequestConfirmationPopupViewModel RequestConfirmationPopupVM { get; private set; }
        public RequestSentPopupViewModel RequestSentPopupVM { get; private set; }
        public RequestDateAndTimePopupViewModel RequestDateAndTimePopupVM { get; private set; }


        private bool _isAddressGettingSaved;
        public bool IsAddressGettingSaved
        {
            get { return _isAddressGettingSaved; }
            set { Set(ref _isAddressGettingSaved, value); }
        }

        private string _requestTypeHeader;

        public string RequestTypeHeader
        {
            get { return _requestTypeHeader; }
            set { Set(ref _requestTypeHeader, value); }
        }


        private double _initialMapLat;
        public double InitialMapLat
        {
            get { return _initialMapLat; }
            set { Set(ref _initialMapLat, value); }
        }
        private double _initialMapLng;

        public double InitialMapLng
        {
            get { return _initialMapLng; }
            set { Set(ref _initialMapLng, value); }
        }

        private RequestModel _currentRequest;

        public RequestModel CurrentRequest
        {
            get { return _currentRequest; }
            set { _currentRequest = value; }
        }


        private AddressModel _currentAddress;

        public AddressModel CurrentAddress
        {
            get { return _currentAddress; }
            set { Set(ref _currentAddress, value); }
        }

        private string _commentsLenghtReminder = "0/120";

        public string CommentsLenghtReminder
        {
            get { return _commentsLenghtReminder; }
            set { Set(ref _commentsLenghtReminder, value); }
        }


        private string _requestComments;

        public string RequestComments
        {
            get { return _requestComments; }
            set { Set(ref _requestComments, value); }
        }


        private DateTime _dateToApply = DateTime.Now;

        public DateTime DateToApply
        {
            get { return _dateToApply; }
            set { Set(ref _dateToApply, value); }
        }


        private string _dateToApplyParsed;

        public string DateToApplyParsed
        {
            get { return _dateToApplyParsed; }
            set { Set(ref _dateToApplyParsed, value); }
        }


        private string _timeToApplyParsed;
        public string TimeToApplyParsed
        {
            get { return _timeToApplyParsed; }
            set { Set(ref _timeToApplyParsed, value); }
        }



        private TimeSpan _timeToApply = new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second);

        public TimeSpan TimeToApply
        {
            get { return _timeToApply; }
            set { Set(ref _timeToApply, value); }
        }





        #endregion



    }
}