using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace AeccApp.Core.ViewModels.Popups
{
   public class ChatLeaseConversationPopupViewModel : ViewModelBase
    {
        public event EventHandler LeaseChatConversation;

        private Command _yesCommand;
        public ICommand YesCommand
        {
            get
            {
                return _yesCommand ??
                    (_yesCommand = new Command(o => LeaseChatConversation?.Invoke(this, null)));
            }
        }

        private Command _noCommand;
        public ICommand NoCommand
        {
            get
            {
                return _noCommand ??
                    (_noCommand = new Command(OnLeaseConversationPopupClose));
            }
        }
        private async void OnLeaseConversationPopupClose()
        {
            await NavigationService.HidePopupAsync();
        }
    }
}
