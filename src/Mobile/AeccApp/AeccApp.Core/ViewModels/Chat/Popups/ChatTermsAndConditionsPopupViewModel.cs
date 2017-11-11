using AeccApp.Core.Messages;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace AeccApp.Core.ViewModels.Popups
{
    public class ChatTermsAndConditionsPopupViewModel : ViewModelBase
    {

        private Command _acceptTermsCommand;
        public ICommand AcceptTermsCommand
        {
            get
            {
                return _acceptTermsCommand ??
                    (_acceptTermsCommand = new Command(OnAcceptTermsCommand, o => !IsBusy));
            }
        }

        private async void OnAcceptTermsCommand(object obj)
        {
            Settings.TermsAndConditionsAccept = true;
            await NavigationService.HidePopupAsync();
            //TODO REFRESH CHAT TAB

        }

        private Command _rejectTermsCommand;
        public ICommand RejectTermsCommand
        {
            get
            {
                return _rejectTermsCommand ??
                    (_rejectTermsCommand = new Command(OnRejectTermsCommand, o => !IsBusy));
            }
        }

        private async void OnRejectTermsCommand(object obj)
        {
            await NavigationService.HidePopupAsync();
            MessagingCenter.Send(new DashboardTabMessage(TabsEnum.Home), string.Empty);

        }


    }
}
