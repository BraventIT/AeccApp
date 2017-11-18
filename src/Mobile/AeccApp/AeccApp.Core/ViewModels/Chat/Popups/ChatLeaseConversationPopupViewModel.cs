using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace AeccApp.Core.ViewModels.Popups
{
    public class ChatLeaseConversationPopupViewModel : ClosablePopupViewModelBase
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
    }
}