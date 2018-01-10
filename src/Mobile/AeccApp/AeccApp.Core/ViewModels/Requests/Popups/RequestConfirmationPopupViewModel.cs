using System;
using System.Windows.Input;
using Xamarin.Forms;

namespace AeccApp.Core.ViewModels.Popups
{
    public class RequestConfirmationPopupViewModel : ClosablePopupViewModelBase
    {
        public event EventHandler ConfirmRequestToSend;

        private Command _sendRequestConfirmationCommand;
        public ICommand SendRequestConfirmationCommand
        {
            get
            {
                return _sendRequestConfirmationCommand ??
                    (_sendRequestConfirmationCommand = new Command(o => ConfirmRequestToSend?.Invoke(this, null)));
            }
        }
  

        private string _displayRequestInfo;

        public string DisplayRequestInfo
        {
            get { return _displayRequestInfo; }
            set { Set(ref _displayRequestInfo, value); }
        }

        private string _displayDate;

        public string DisplayDate
        {
            get { return _displayDate; }
            set { Set(ref _displayDate, value); }
        }

        private string _displayTime;

        public string DisplayTime
        {
            get { return _displayTime; }
            set { Set(ref _displayTime, value); }
        }
    }
}
