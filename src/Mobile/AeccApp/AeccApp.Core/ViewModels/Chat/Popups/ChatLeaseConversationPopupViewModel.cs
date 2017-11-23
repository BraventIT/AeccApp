using System;
using System.Windows.Input;
using Xamarin.Forms;

namespace AeccApp.Core.ViewModels.Popups
{
    public class ChatLeaseConversationPopupViewModel : ClosablePopupViewModelBase
    {
        public event EventHandler LeaseChatConversation;

        private Command _leaseChatCommand;
        public ICommand LeaseChatCommand
        {
            get
            {
                return _leaseChatCommand ??
                    (_leaseChatCommand = new Command(o => LeaseChatConversation?.Invoke(this, null)));
            }
        }
    }
}