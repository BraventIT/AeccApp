using AeccApp.Core.Messages;
using AeccBot.MessageRouting;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace AeccApp.Core.ViewModels
{
    public class ChatEventViewModel : ViewModelBase
    {
        private ChatEventMessage eventMessage;
        public ChatEventMessage Event
        {
            get { return eventMessage; }
            set { Set(ref eventMessage, value); }
        }


        public override async Task InitializeAsync(object navigationData)
        {
            if (navigationData is ChatEventMessage)
            {
                Event = navigationData as ChatEventMessage;

                switch (Event.Type)
                {
                    case MessageRouterResultType.ConnectionRejected:
                        if (GlobalSetting.Instance.User.IsVolunteer == false)
                        {
                            Event.MessageTitle = LocalizationResourceManager.GetString("PopupChatEventConnectionRejectedNotVolunteer");
                        }
                        else
                        {
                            Event.MessageTitle = LocalizationResourceManager.GetString("PopupChatEventConnectionRejectedVolunteer");
                        }
                        break;
                    case MessageRouterResultType.DeleteAggregation:
                        Event.MessageTitle = LocalizationResourceManager.GetString("PopupChatEventDeleteAggregation");
                        break;
                    case MessageRouterResultType.Disconnected:
                        Event.MessageTitle = LocalizationResourceManager.GetString("PopupChatEventDisconnected");
                        break;
                    default:
                        Event.MessageTitle = LocalizationResourceManager.GetString("PopupChatEventSorry");
                        break;
                }
            }
            else
            {
                await NavigationService.NavigateBackAsync();
            }
        }

        #region Popups Commands

        private Command okCommand;
        public ICommand OkCommand
        {
            get
            {
                return okCommand ??
                    (okCommand = new Command((o) => OnOkEventAsync()));
            }
        }

        private Task OnOkEventAsync()
        {
            return NavigationService.NavigateBackAsync();
        }
        #endregion

    }
}
