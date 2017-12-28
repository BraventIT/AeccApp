using Aecc.Models;
using AeccApp.Core.Extensions;
using AeccApp.Core.Messages;
using AeccApp.Core.Models;
using AeccApp.Core.Services;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace AeccApp.Core.ViewModels
{
    class HomeViewModel : ViewModelBase
    {
        private const int NUM_NEWS = 3;

        private IChatService ChatService { get; } = ServiceLocator.ChatService;
        private INewsDataService NewsDataService { get; } = ServiceLocator.NewsDataService;
        private INewsRequestService NewsService { get; } = ServiceLocator.NewsService;

        #region Contructor & Initialize
        public HomeViewModel()
        {
            IsHeaderInfoBannerVisible = Settings.HomeHeaderBannerClosed;
            NewsList = new ObservableCollection<NewsModel>();
        }

        public override async Task ActivateAsync()
        {
            MessagingCenter.Subscribe<ChatMessageReceivedMessage>(this, string.Empty, OnMessageReceived);
            MessagingCenter.Subscribe<ChatStateMessage>(this, string.Empty, OnChatState);

            var toolbarMessage = (Device.RuntimePlatform != Device.UWP) ?
                new ToolbarMessage(true) :
                new ToolbarMessage(this["DashboardAppTitle"]);

            MessagingCenter.Send(toolbarMessage, string.Empty);

            await ExecuteOperationAsync(async () =>
            {
                VolunteerIsActive = ChatService.VolunteerIsActive;
                InConversation = ChatService.InConversation;
                LastMessage = (ChatService.MessagesWitoutReading) ?
                  ChatService.GetConversationMessages().Last() : null;

                if (!NewsList.Any())
                    await FillNewsAsync();
            });
            ServiceLocator.ChatMessagesListenerService.InitChatMessageListener();
            await ExecuteOperationQuietlyAsync(cancelToken => TryToUpdateNewsAsync(cancelToken));
        }

        public override void Deactivate()
        {
            MessagingCenter.Unsubscribe<ChatMessageReceivedMessage>(this, string.Empty);
            MessagingCenter.Unsubscribe<ChatStateMessage>(this, string.Empty);
            try
            {
                MessagingCenter.Send(new ToolbarMessage(false), string.Empty);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }

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

        private bool _inConversation;
        public bool InConversation
        {
            get { return _inConversation; }
            set
            {
                Set(ref _inConversation, value);
            }
        }

        private Message _lastMessage;
        public Message LastMessage
        {
            get { return _lastMessage; }
            set { Set(ref _lastMessage, value); }
        }


        private ObservableCollection<NewsModel> _newsList;
        public ObservableCollection<NewsModel> NewsList
        {
            get { return _newsList; }
            set { Set(ref _newsList, value); }
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

        void OnOpenAllRequest()
        {

            MessagingCenter.Send(new DashboardTabMessage(TabsEnum.Requests), string.Empty);
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
            if (Settings.FirstRequestLandingPageSeen)
            {
                MessagingCenter.Send(new DashboardTabMessage(TabsEnum.Requests), string.Empty);
            }
            else
            {
                NavigationService.NavigateToAsync<NewRequestSelectAddressViewModel>();
            }


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
                    (openAllNewsCommand = new Command(o => OnOpenAllNewsViewAsync()));
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
        private async Task FillNewsAsync()
        {
            var news = (await NewsDataService.GetListAsync()).Take(NUM_NEWS).ToList();
            if (news.Any())
            {
                NewsList.SyncExact(news);
            }
        }

        private async Task TryToUpdateNewsAsync(CancellationToken cancelToken)
        {
            var today = DateTime.Today.ToUniversalTime();
            if (Settings.LastNewsChecked != today || !NewsList.Any())
            {
                var news = await NewsService.GetNewsAsync(cancelToken, NUM_NEWS);

                foreach (var newData in news.Reverse())
                {
                    await NewsDataService.InsertOrUpdateAsync(newData);
                }
                NewsList.SyncExact(news);

                Settings.LastNewsChecked = today;
            }
        }

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
