using AeccApp.Core.Messages;
using AeccApp.Core.Services;
using AeccBot.Models;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace AeccApp.Core.ViewModels
{
    public class ChatRequestViewModel : ViewModelBase
    {
        private readonly IChatService ChatService;

        private string _requestPartyId;

        #region Contructor & Initialize
        public ChatRequestViewModel()
        {
            ChatService = ServiceLocator.Resolve<IChatService>();
        }

        public override async Task InitializeAsync(object navigationData)
        {
            if (navigationData is string)
            {
                var party = Party.FromJsonString((string)navigationData);
                if (party != null)
                {
                    _requestPartyId = (string)navigationData;
                    RequestMessage = $"Usuario: {party.ChannelAccount.Name}";
                }
            }
            else
            {
                await NavigationService.NavigateBackAsync();
            }
        }

        public override void Deactivate()
        {
            MessagingCenter.Unsubscribe<ChatEventMessage>(this, string.Empty);
        }

        public override Task ActivateAsync()
        {
            MessagingCenter.Subscribe<ChatEventMessage>(this, string.Empty, OnChatEvent);

            return ExecuteOperationAsync(() => ChatService.InitializeAsync());
        }
        #endregion

        #region Properties
        private string _requestMessage;

        public string RequestMessage
        {
            get { return _requestMessage; }
            set { Set(ref _requestMessage, value); }
        }
        #endregion

        #region Commands
        private Command _acceptRequestCommand;
        public ICommand AcceptRequestCommand
        {
            get
            {
                return _acceptRequestCommand ??
                    (_acceptRequestCommand = new Command((o) => OnAcceptRequestAsync(), (o) => !IsBusy));
            }
        }

        private Task OnAcceptRequestAsync()
        {
            return ExecuteOperationAsync(
                async () =>
                {
                    try
                    {
                        if (await ChatService.AcceptAndRejectRequestAsync(true, _requestPartyId))
                        {
                            await NavigationService.NavigateBackAsync();
                            // Wait for load back page
                            await Task.Delay(200);
                            MessagingCenter.Send(new DashboardTabMessage(TabsEnum.Chat), string.Empty);
                        }
                    }
                    catch
                    {
                        await NavigationService.NavigateBackAsync();
                    }
                });
        }

        private Command _rejectRequestCommand;
        public ICommand RejectRequestCommand
        {
            get
            {
                return _rejectRequestCommand ??
                    (_rejectRequestCommand = new Command((o) => OnRejectRequestAsync(), (o) => !IsBusy));
            }
        }

        private Task OnRejectRequestAsync()
        {
            return ExecuteOperationAsync(
                () => ChatService.AcceptAndRejectRequestAsync(false, _requestPartyId)
                , finallyAction: () => NavigationService.NavigateBackAsync());
        }
        #endregion

        #region Private Methods
        private async void OnChatEvent(ChatEventMessage obj)
        {
            await NavigationService.NavigateBackAsync();
            // Wait for load back page
            await Task.Delay(200);
            // Resend ChatEvent message
            MessagingCenter.Send(obj, string.Empty);
        }

        protected override void OnIsBusyChanged()
        {
            _acceptRequestCommand?.ChangeCanExecute();
            _rejectRequestCommand?.ChangeCanExecute();
        }
        #endregion

        public override bool OnBackButtonPressed()
        {
            return true;
        }
    }
}
