using AeccApp.Core.Messages;
using AeccApp.Core.Models;
using AeccApp.Core.Services;
using AeccApp.Core.ViewModels.Popups;
using System;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace AeccApp.Core.ViewModels
{
    public class CompletingHospitalRequestViewModel : ViewModelBase
    {
        private IAddressesDataService AddressesDataService { get; } = ServiceLocator.AddressesDataService;
        private IHospitalRequestDataService HospitalRequestDataService { get; } = ServiceLocator.HospitalRequestDataService;
        private IEmailService EmailService { get; } = ServiceLocator.EmailService;
        private IHospitalRequestService HospitalRequestService { get; } = ServiceLocator.HospitalRequestService;


        #region Constructor and initialization

        public CompletingHospitalRequestViewModel()
        {
            DateAndTimeButtonText = this["CompletingRequestTimeAndDate"];
            RequestDateAndTimePopupVM = new RequestDateAndTimePopupViewModel();
            RequestConfirmationPopupVM = new RequestConfirmationPopupViewModel();
            RequestSentPopupVM = new RequestSentPopupViewModel();
        }

        public override Task InitializeAsync(object navigationData)
        {
            CurrentRequest = navigationData as RequestModel;
            CurrentAddress = CurrentRequest.RequestAddress;
            if (CurrentAddress.HospitalRoom == null || CurrentAddress.HospitalRoom == string.Empty)
            {
                HasUserFilledRoomForm = false;
            }
            RequestTypeHeader = CurrentRequest.RequestType.Name;
            DisplayRequestInfo = $"Tu petición es:\n \"{CurrentRequest.RequestType.Name }\"\n{CurrentRequest.RequestAddress.DisplayAddress}";

            return Task.CompletedTask;
        }

        public override Task ActivateAsync()
        {
            RequestDateAndTimePopupVM.ApplyDateAndTime += OnApplyDateAndTimeCommand;
            RequestConfirmationPopupVM.ConfirmRequestToSend += OnSendRequestConfirmationCommand;

            MessagingCenter.Send(new GeolocatorMessage(CurrentAddress.Coordinates), string.Empty);
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
                    (_mapDetailCommand = new Command(OnMapDetailCommand, o => !IsBusy));
            }
        }

        private async void OnMapDetailCommand(object obj)
        {
            await NavigationService.NavigateToAsync<MapDetailViewModel>(CurrentAddress.Coordinates);
        }


        private Command _openRequestConfirmationPopupCommand;
        public ICommand OpenRequestConfirmationPopupCommand
        {
            get
            {
                return _openRequestConfirmationPopupCommand ??
                    (_openRequestConfirmationPopupCommand = new Command(OnOpenRequestConfirmationPopupCommand, o => !IsBusy));
            }
        }

        private async void OnOpenRequestConfirmationPopupCommand(object obj)
        {
            if (DateToApplyParsed == null)
            {
                DateToApplyParsed = DateTime.Now.ToString().Remove(10);
            }

            if (TimeToApplyParsed == null)
            {
                TimeToApplyParsed = new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second).ToString().Remove(5);
            }

            RequestConfirmationPopupVM.DisplayRequestInfo = DisplayRequestInfo;
            RequestConfirmationPopupVM.DisplayDate = DateToApplyParsed + "," + TimeToApplyParsed;

            await NavigationService.ShowPopupAsync(RequestConfirmationPopupVM);


        }


        private Command _openDateAndTimePopupCommand;
        public ICommand OpenDateAndTimePopupCommand
        {
            get
            {
                return _openDateAndTimePopupCommand ??
                    (_openDateAndTimePopupCommand = new Command(OnOpenDateAndTimePopupCommand, o => !IsBusy));
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
            DateAndTimeButtonText = DateToApplyParsed + "," + TimeToApplyParsed;
            await NavigationService.HidePopupAsync();

        }

        private Command _commentsEntryListenerCommand;
        public ICommand CommentsEntryListenerCommand
        {
            get
            {
                return _commentsEntryListenerCommand ??
                    (_commentsEntryListenerCommand = new Command(OnCommentsEntryListenerCommand, o => !IsBusy));
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

            CurrentRequest.RequestTimeStamp = new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second).ToString().Remove(5);

            CurrentRequest.RequestAddress.IsHospitalAddress = true;
            await HospitalRequestDataService.InsertOrUpdateAsync(CurrentRequest);
            if (IsAddressGettingSaved)
            {
                await AddressesDataService.InsertOrUpdateAsync(CurrentRequest.RequestAddress);
            }
            await NavigationService.HidePopupAsync();

            //TODO UNCOMMENT WHEN BACK OFFICE HAS FILLED COORDINATORS EMAILS 
            //await ExecuteOperationAsync(async cancelToken =>
            //{
            //    var currentHospitalSelected = await HospitalRequestService.GetHospitalDetail(CurrentAddress.HospitalID, cancelToken);
            //    List<HospitalAssignment> HospitalAssignments = new List<HospitalAssignment>();
            //    HospitalAssignments.AddRange(currentHospitalSelected.HospitalAssignments); 
            //    string[] coordinatorsEmails = new string[HospitalAssignments.Count];
            //    for (int i = 0; i < currentHospitalSelected.HospitalAssignments.Count ; i++)
            //    {
            //        coordinatorsEmails[i] = HospitalAssignments[i].Coordinator.Email;
            //    }
            //    await OnSendRequestAsync(coordinatorsEmails);
            //});

            //LEAVE THIS FOR EMAIL TESTING
            string[] emails = GlobalSetting.TEST_EMAIL.Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
            await OnSendRequestAsync(emails);
        }


        #endregion

        #region Properties

        public RequestConfirmationPopupViewModel RequestConfirmationPopupVM { get; private set; }
        public RequestSentPopupViewModel RequestSentPopupVM { get; private set; }
        public RequestDateAndTimePopupViewModel RequestDateAndTimePopupVM { get; private set; }

        private string _dateAndTimeButtonText ;

        public string DateAndTimeButtonText
        {
            get { return _dateAndTimeButtonText; }
            set { Set(ref _dateAndTimeButtonText, value); }
        }


        private bool _hasUserFilledRoomForm = true;

        public bool HasUserFilledRoomForm
        {
            get { return _hasUserFilledRoomForm; }
            set { Set(ref _hasUserFilledRoomForm, value); }
        }


        private string _displayRequestInfo;

        public string DisplayRequestInfo
        {
            get { return _displayRequestInfo; }
            set { Set(ref _displayRequestInfo, value); }
        }

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

        #region Methods
        private Task OnSendRequestAsync(string[] receiversAddresses)
        {
            return ExecuteOperationAsync(
                executeAction: async cancelToken =>
                {
                    var email = new EmailFromHospital(CurrentRequest, receiversAddresses);
                    await EmailService.SendAsync(email, cancelToken);

                },
                finallyAction: async () =>
                {
                    await NavigationService.HidePopupAsync();
                    await NavigationService.ShowPopupAsync(RequestSentPopupVM);
                });
        }
        #endregion
    }
}