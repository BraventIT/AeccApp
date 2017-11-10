using AeccApp.Core.Extensions;
using AeccApp.Core.Messages;
using AeccApp.Core.Models;
using AeccApp.Core.Services;
using AeccApp.Core.ViewModels.Popups;
using Plugin.Permissions.Abstractions;
using System;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace AeccApp.Core.ViewModels
{
    public class DashboardViewModel : ViewModelBase
    {
        private IChatService ChatService { get; } = ServiceLocator.ChatService;
        private IIdentityService IdentityService { get; } = ServiceLocator.IdentityService;
        private IPermissions PermissionsService { get; } = ServiceLocator.PermissionsService;

        #region Contructor & Initialize

        public DashboardViewModel()
        {
            NoLocationProviderPopupVM = new NoLocationProviderPopupViewModel();
        }

        public override async Task InitializeAsync(object navigationData)
        {
            if (navigationData is TabParameter)
            {
                // Change selected application tab
                var tabIndex = ((TabParameter)navigationData).TabIndex;
                MessagingCenter.Send(new TabMessage(tabIndex), string.Empty);
            }


        }

        public override Task ActivateAsync()
        {
            MessagingCenter.Subscribe<ChatEventMessage>(this, string.Empty, o => OnChatEventAsync(o));
            MessagingCenter.Subscribe<ChatEngagementEventMessage>(this, string.Empty, o => OnChatEngagementEventAsync(o));

            if (IsVolunteer)
            {
                //review
              //  MessagingCenter.Send(new DashboardEnableAndDisableChatTab(false), string.Empty);
                MessagingCenter.Send(new DashboardHideRequestsTabMessage(TabsEnum.Requests), string.Empty);
            }

            return ExecuteOperationAsync(async (token) =>
             {
                 await ExecuteOperationAsync(async () =>
                 {
                     await ChatService.InitializeAsync();
                     InConversation = ChatService.InConversation;
                 });

                 try
                 {
                     var hasPermission = await PermissionsService.CheckPermissionsAsync(Permission.Location);
                     if (!hasPermission)
                         return;
                 }
                 catch (Exception)
                 {
                     await NavigationService.ShowPopupAsync(NoLocationProviderPopupVM);
                 }

                 //await ServiceLocator.EmailService.SendAsync(new AeccApi.Models.EmailMessage()
                 //{
                 //    To = $"{GlobalSetting.Instance.User.Email};afraj@bravent.net",
                 //    Subject = "Prueba de envío de mensajes",
                 //    Body = $"Esto es una prueba.\r\nSe ha enviado en nombre de: {GlobalSetting.Instance.User.Name} {GlobalSetting.Instance.User.Surname}\r\n\r\nUn saludo!! ;-)"
                 //}, token);
             });
        }

        public override void Deactivate()
        {
            MessagingCenter.Unsubscribe<ChatEventMessage>(this, string.Empty);
            MessagingCenter.Unsubscribe<ChatEngagementEventMessage>(this, string.Empty);
        }
        #endregion

        #region Properties
        public NoLocationProviderPopupViewModel NoLocationProviderPopupVM { get; private set; }

        private bool inConversation;
        public bool InConversation
        {
            get { return inConversation; }
            set
            {
                Set(ref inConversation, value);
            }
        }

        #endregion

        #region Commands

        private Command logOffCommand;
        public ICommand LogOffCommand
        {
            get
            {
                return logOffCommand ??
                    (logOffCommand = new Command(OnLogOffAsync));
            }

        }
        /// <summary>
        /// Logout
        /// </summary>
        /// <param name="obj"></param>
        private async void OnLogOffAsync(object obj)
        {
            IdentityService.LogOff();
            await NavigationService.NavigateToAsync<LoginViewModel>();
            await NavigationService.RemoveBackStackAsync();
        }

        private Command openNotificationsCommand;
        public ICommand OpenNotificationsCommand
        {
            get
            {
                return openNotificationsCommand ??
                    (openNotificationsCommand = new Command(OnNotificationsOpened));
            }
        }
        private void OnNotificationsOpened(object obj)
        {
            //TODO Navigate to notification view
        }

        private Command newChatCommand;
        public ICommand NewChatCommand
        {
            get
            {
                return newChatCommand ??
                    (newChatCommand = new Command(NewChatCommandAsync));
            }
        }

        private void NewChatCommandAsync(object obj)
        {
            MessagingCenter.Send(this, "New");
        }
        #endregion

        #region Private Methods
        private async Task OnChatEngagementEventAsync(ChatEngagementEventMessage chatEngagementEvent)
        {
            await NavigationService.NavigateToAsync<ChatRequestViewModel>(chatEngagementEvent.RequestPartyId, isModal: true);
        }

        private async Task OnChatEventAsync(ChatEventMessage obj)
        {
            await NavigationService.NavigateToAsync<ChatEventViewModel>(obj, isModal: true);
        }
        #endregion
    }
}
