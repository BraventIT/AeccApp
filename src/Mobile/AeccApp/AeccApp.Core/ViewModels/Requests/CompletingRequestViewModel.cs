using AeccApp.Internationalization.Properties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Resources;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace AeccApp.Core.ViewModels
{
    public class CompletingRequestViewModel : ViewModelBase
    {
      



        #region Commands
        private Command _closeRequestSentPopupCommand;
        public ICommand CloseRequestSentPopupCommand
        {
            get
            {
                return _closeRequestSentPopupCommand ??
                    (_closeRequestSentPopupCommand = new Command(OnCloseRequestSentPopupCommand, (o) => !IsBusy));
            }
        }
        private void OnCloseRequestSentPopupCommand(object obj)
        {

            
            IsRequestSentPopupVisible = false;

        }

        private Command _sendRequestCommand;
        public ICommand SendRequestCommand
        {
            get
            {
                return _sendRequestCommand ??
                    (_sendRequestCommand = new Command(OnSendRequestCommand, (o) => !IsBusy));
            }
        }

        private void OnSendRequestCommand(object obj)
        {

            //TODO send request
            IsRequestConfirmationPopupVisible = false;
            IsRequestSentPopupVisible = true;
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

        private void OnOpenRequestConfirmationPopupCommand(object obj)
        {
            if (DateToApplyParsed==null)
            {
                DateToApplyParsed = DateTime.Now.ToString().Remove(10);
            }
            if (TimeToApplyParsed == null)
            {
                TimeToApplyParsed = new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second).ToString().Remove(5);
            }


            IsRequestConfirmationPopupVisible = true;

        }


        private Command _closeRequestConfirmationPopupCommand;
        public ICommand CloseRequestConfirmationPopupCommand
        {
            get
            {
                return _closeRequestConfirmationPopupCommand ??
                    (_closeRequestConfirmationPopupCommand = new Command(OnCloseRequestConfirmationPopupCommand, (o) => !IsBusy));
            }
        }

        private void OnCloseRequestConfirmationPopupCommand(object obj)
        {
            IsRequestConfirmationPopupVisible = false;
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

        private void OnOpenDateAndTimePopupCommand(object obj)
        {
            IsDateAndTimePopupVisible = true;
        }

        private Command _closeDateAndTimePopupCommand;
        public ICommand CloseDateAndTimePopupCommand
        {
            get
            {
                return _closeDateAndTimePopupCommand ??
                    (_closeDateAndTimePopupCommand = new Command(OnCloseDateAndTimePopupCommand, (o) => !IsBusy));
            }
        }

        private void OnCloseDateAndTimePopupCommand(object obj)
        {
            IsDateAndTimePopupVisible = false;
        }


        private Command _applyDateAndTimeCommand;
        public ICommand ApplyDateAndTimeCommand
        {
            get
            {
                return _applyDateAndTimeCommand ??
                    (_applyDateAndTimeCommand = new Command(OnApplyDateAndTimeCommand, (o) => !IsBusy));
            }
        }

        private void OnApplyDateAndTimeCommand(object obj)
        {
            //Apply date and time to the request:
            DateToApplyParsed = DateToApply.Date.ToString().Remove(10);
            TimeToApplyParsed = TimeToApply.ToString().Remove(5);
            IsDateAndTimePopupVisible = false;

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




        #endregion


        #region Properties

        private float _initialMapLat;

        public float InitialMapLat
        {
            get { return _initialMapLat; }
            set { _initialMapLat = value; }
        }
        private float _initialMapLng;

        public float InitialMapLng
        {
            get { return _initialMapLng; }
            set { _initialMapLng = value; }
        }


        private bool _isRequestSentPopupVisible;

        public bool IsRequestSentPopupVisible
        {
            get { return _isRequestSentPopupVisible; }
            set { Set(ref _isRequestSentPopupVisible, value); }
        }


        private bool _isRequestConfirmationPopupVisible;

        public bool IsRequestConfirmationPopupVisible
        {
            get { return _isRequestConfirmationPopupVisible; }
            set { Set(ref _isRequestConfirmationPopupVisible, value); }
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


        private bool _isDateAndTimePopupVisible;

        public bool IsDateAndTimePopupVisible
        {
            get { return _isDateAndTimePopupVisible; }
            set { Set(ref _isDateAndTimePopupVisible, value); }
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
        public override bool OnBackButtonPressed()
        {
            bool returnValue = false;
            if (IsRequestSentPopupVisible)
            {
                IsRequestSentPopupVisible = false;
                returnValue = true;
            }
            else if (IsRequestConfirmationPopupVisible)
            {
                IsRequestConfirmationPopupVisible = false;
                returnValue = true;
            }
            else if (IsDateAndTimePopupVisible)
            {
                IsDateAndTimePopupVisible = false;
                returnValue = true;
            }
            return returnValue;
        }
        #endregion






    }




}
