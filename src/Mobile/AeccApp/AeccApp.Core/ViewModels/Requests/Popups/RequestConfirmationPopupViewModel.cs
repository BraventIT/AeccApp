using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace AeccApp.Core.ViewModels.Popups
{
   public class RequestConfirmationPopupViewModel : ViewModelBase
    {

        private Command _closePopupCommand;
        public ICommand ClosePopupCommand
        {
            get
            {
                return _closePopupCommand ??
                    (_closePopupCommand = new Command(OnClosePopupCommand));
            }
        }
        private async void OnClosePopupCommand()
        {
            await NavigationService.HidePopupAsync();
        }

        private Command _sendRequestConfirmationCommand;
        public ICommand SendRequestConfirmationCommand
        {
            get
            {
                return _sendRequestConfirmationCommand ??
                    (_sendRequestConfirmationCommand = new Command(OnSendRequestConfirmationCommand));
            }
        }
        private async void OnSendRequestConfirmationCommand()
        {
            await NavigationService.HidePopupAsync();
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
