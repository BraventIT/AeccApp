using Aecc.Models;
using AeccApp.Core.Messages;
using AeccApp.Core.Models;
using AeccApp.Core.Services;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace AeccApp.Core.ViewModels
{
    class HomeViewModel : ViewModelBase
    {
        private IChatService ChatService { get; } = ServiceLocator.ChatService;
        private INewsRequestService NewsService { get; } = ServiceLocator.NewsService;

        #region Contructor & Initialize
        public HomeViewModel()
        {
            IsHeaderInfoBannerVisible = Settings.HomeHeaderBannerClosed;
        }

        public override async Task ActivateAsync()
        {
            MessagingCenter.Subscribe<ChatMessageReceivedMessage>(this, string.Empty, OnMessageReceived);
            MessagingCenter.Subscribe<ChatStateMessage>(this, string.Empty, OnChatState);

            MessagingCenter.Send(new ToolbarMessage(true), string.Empty);

            await ExecuteOperationAsync(async cancelToken =>
            {
                await ChatService.InitializeAsync();

                VolunteerIsActive = ChatService.VolunteerIsActive;
                InConversation = ChatService.InConversation;
                NewsList = await NewsService.GetNewsAsync(cancelToken);
            });
        }

        public override void Deactivate()
        {
            MessagingCenter.Unsubscribe<ChatMessageReceivedMessage>(this, string.Empty);
            MessagingCenter.Unsubscribe<ChatStateMessage>(this, string.Empty);
        }
        #endregion

        #region Propeties

        public string UserName
        {
            get { return GSetting.User?.Name; }
        }

        private bool _isHeaderInfoBannerVisible;
        public bool IsHeaderInfoBannerVisible
        {
            get { return _isHeaderInfoBannerVisible; }
            set
            {
                Set(ref _isHeaderInfoBannerVisible, value);
            }
        }

        private bool inConversation;
        public bool InConversation
        {
            get { return inConversation; }
            set
            {
                Set(ref inConversation, value);
            }
        }

        private Message lastMessage;
        public Message LastMessage
        {
            get { return lastMessage; }
            set { Set(ref lastMessage, value); }
        }


        private IEnumerable<NewsModel> newsList;
        public IEnumerable<NewsModel> NewsList
        {
            get
            {
                return newsList;
            }
            set
            {
                Set(ref newsList, value);
                
            }
        }

        private bool _volunteerIsActive;
        public bool VolunteerIsActive
        {
            get { return _volunteerIsActive; }
            set
            {

                if (Set(ref _volunteerIsActive, value) && !IsBusy)
                {
                    OnVolunteerStateChangedAsync(value);
                }
            }
        }

        #endregion

        #region Popups Properties


        private bool eventPopupVisible;
        public bool EventPopupVisible
        {
            get { return eventPopupVisible; }
            set { Set(ref eventPopupVisible, value); }
        }


        private bool checkBoxImageEnabled;

        public bool CheckBoxImageEnabled
        {
            get { return checkBoxImageEnabled; }
            set { checkBoxImageEnabled = value; }
        }


        #endregion

        #region Commands
        private Command _headerInfoBannerCall;
        public ICommand HeaderInfoBannerCall
        {
            get
            {
                return _headerInfoBannerCall ??
                    (_headerInfoBannerCall = new Command(OnHeaderInfoBannerCall));
            }
        }

        void OnHeaderInfoBannerCall()
        {
            Device.OpenUri(new Uri("tel://900100036"));
        }

        private Command _headerInfoBannerClose;
        public ICommand HeaderInfoBannerClose
        {
            get
            {
                return _headerInfoBannerClose ??
                    (_headerInfoBannerClose = new Command(OnHeaderInfoBannerClose));
            }
        }

        void OnHeaderInfoBannerClose()
        {
            IsHeaderInfoBannerVisible = false;
            Settings.HomeHeaderBannerClosed = false;

        }

        private Command _openAllRequestsCommand;
        public ICommand OpenAllRequestsCommand
        {
            get
            {
                return _openAllRequestsCommand ??
                    (_openAllRequestsCommand = new Command(OnOpenAllRequest));
            }
        }

        async void OnOpenAllRequest()
        {
            //TODO Navigate to AllYourRequestListView if there are active requests
            //TODO Navigate to RequestsView if there are not active requests

            await NavigationService.NavigateToAsync<AllYourRequestsListViewModel>();
        }

        private Command _newRequestCommand;
        public ICommand NewRequestCommand
        {
            get
            {
                return _newRequestCommand ??
                    (_newRequestCommand = new Command(OnNewRequest));
            }
        }
        /// <summary>
        /// Navigates to NewRequestSelectAddressView
        /// </summary>
        void OnNewRequest()
        {
            NavigationService.NavigateToAsync<NewRequestSelectAddressViewModel>();
        }
        private Command _currentChatCommand;
        public ICommand CurrentChatCommand
        {
            get
            {
                return _currentChatCommand ??
                    (_currentChatCommand = new Command(OnCurrentChatCommand));
            }
        }

        void OnCurrentChatCommand()
        {
            MessagingCenter.Send(new DashboardTabMessage(TabsEnum.Chat), string.Empty);
        }

        private Command _newChatCommand;
        public ICommand NewChatCommand
        {
            get
            {
                return _newChatCommand ??
                    (_newChatCommand = new Command(OnNewChat));
            }
        }
     

        async void OnNewChat()
        {
          
                MessagingCenter.Send(new DashboardTabMessage(TabsEnum.Chat), string.Empty);
            
        }


        private Command openAllNewsCommand;
        public ICommand OpenAllNewsCommand
        {
            get
            {
                return openAllNewsCommand ??
                    (openAllNewsCommand = new Command(o=> OnOpenAllNewsViewAsync()));
            }
        }
        /// <summary>
        /// Navigates to AllNewsView
        /// </summary>
        private async Task OnOpenAllNewsViewAsync()
        {
            await NavigationService.NavigateToAsync<AllNewsViewModel>();
        }

        private Command chooseNewCommand;
        public ICommand ChooseNewCommand
        {
            get
            {
                return chooseNewCommand ??
                    (chooseNewCommand = new Command(o => OnChooseNewAsync(o), o => !IsBusy));
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj">obj contains the NewsList item tapped</param>
        private async Task OnChooseNewAsync(object obj)
        {
            var selectedNew = obj as NewsModel;
            await NavigationService.NavigateToAsync<NewsDetailViewModel>(selectedNew);
        }

        #endregion

        #region Private Methods

        public override bool OnBackButtonPressed()
        {
            bool returnValue = false;
            if (EventPopupVisible)
            {
                EventPopupVisible = false;
                returnValue = true;
            }
         

            return returnValue;
        }


        private void OnMessageReceived(ChatMessageReceivedMessage received)
        {
            LastMessage = received.Message;
        }

        private void OnChatState(ChatStateMessage obj)
        {
            InConversation = obj.InConversation;
            VolunteerIsActive = obj.VolunteerIsActive;
        }

        Task OnVolunteerStateChangedAsync(bool isActive)
        {
            return ExecuteOperationAsync(() => ChatService.SetVolunteerState(isActive),
                finallyAction: () => VolunteerIsActive = ChatService.VolunteerIsActive);
        }
        #endregion
    }
}
