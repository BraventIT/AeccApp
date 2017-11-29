using AeccApp.Core.Messages;
using AeccApp.Core.Models;
using AeccApp.Core.Services;
using AeccApp.Core.ViewModels.Popups;
using AeccBot.MessageRouting;
using Microsoft.Bot.Connector.DirectLine;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace AeccApp.Core.ViewModels
{
    public class ChatViewModel : ViewModelBase
    {
        const int MIN_MINUTES_INTEVAL = 1;

        private IChatService ChatService { get; } = ServiceLocator.ChatService;
        private IList<UserData> _listVolunteers;
        private DateTime _lastMessageTime;

        #region Contructor & Initialize
        public ChatViewModel()
        {
            Messages = new ObservableCollection<Message>();
            Volunteers = new ObservableCollection<UserData>();

            _filters = ViewModelLocator.ChatFiltersVM;
            ChatRatingPopupVM = new ChatRatingPopupViewModel();
            ChatLeaseConversationPopupVM = new ChatLeaseConversationPopupViewModel();
            ChatTermsAndConditionsPopupVM = new ChatTermsAndConditionsPopupViewModel();
            ChatConnectingPopupVM = new ChatConnectingPopupViewModel();
        }
        //public override Task InitializeAsync(object navigationData)
        //{
        //    if(navigationData!= null)
        //    {
        //        Filters = navigationData as ChatFiltersModel;
        //    }
        //    else
        //    {
        //        Filters = new ChatFiltersModel(18, 80, string.Empty);
        //    }


        //    return Task.CompletedTask;
        //}


        public override async Task ActivateAsync()
        {
            MessagingCenter.Subscribe<ChatStateMessage>(this, string.Empty, OnChatState);
            MessagingCenter.Subscribe<ChatEventMessage>(this, string.Empty, o => OnChatEventAsync(o));

           
            ChatConnectingPopupVM.LeaseChatConversation += OnLeaseConversation;
            ChatLeaseConversationPopupVM.LeaseChatConversation += OnLeaseConversation;
            ChatService.MessagesReceived += OnMesagesReceived;
            ChatService.AggregationsReceived += OnAggregationsReceived;

            if (FirstChat)
            {
                MessagingCenter.Send(new ToolbarMessage(this["FirstChatToolbarTitle"]), string.Empty);
            }
            else
                await InitializeAsync();
        }

        public override void Deactivate()
        {
            MessagingCenter.Unsubscribe<ChatStateMessage>(this, string.Empty);
            MessagingCenter.Unsubscribe<ChatEventMessage>(this, string.Empty);
            ChatConnectingPopupVM.LeaseChatConversation -= OnLeaseConversation;
            ChatLeaseConversationPopupVM.LeaseChatConversation -= OnLeaseConversation;
            ChatService.MessagesReceived -= OnMesagesReceived;
            ChatService.AggregationsReceived -= OnAggregationsReceived;
        }
        #endregion

        #region Properties
        public bool FirstChat
        {
            get { return Settings.FirstChat && !IsVolunteer; }
        }

        private string _partyId;
        public string PartyId
        {
            get { return _partyId; }
            set
            {
                if (Set(ref _partyId, value))
                {
                    NotifyPropertyChanged(nameof(ConversationCounterpart));
                    if (!string.IsNullOrEmpty(_partyId))
                    {
                        CanFilterVolunteers = false;
                    }
                }
            }
        }

        public UserData ConversationCounterpart
        {
            get { return ChatService.ConversationCounterpart; }
        }

        private bool _volunteerIsActive;
        public bool VolunteerIsActive
        {
            get { return _volunteerIsActive; }
            set { Set(ref _volunteerIsActive, value); }
        }

        #region Chat Properties
        private ChatFiltersViewModel _filters;
    
        public ObservableCollection<Message> Messages { get; private set; }

        private string text;
        public string Text
        {
            get { return text; }
            set { Set(ref text, value); }
        }
        #endregion

        #region Choose volunteer Propeties
        public ObservableCollection<UserData> Volunteers { get; private set; }

        private bool _volunteersIsEmpty;
        public bool VolunteersIsEmpty
        {
            get { return _volunteersIsEmpty; }
            set { Set(ref _volunteersIsEmpty, value); }
        }

        private bool _canFilterVolunteers;
        public bool CanFilterVolunteers
        {
            get { return _canFilterVolunteers; }
            set { Set(ref _canFilterVolunteers, value); }
        }

        private bool _filterVolunteersIsEmpty;
        public bool FilterVolunteersIsEmpty
        {
            get { return _filterVolunteersIsEmpty; }
            set { Set(ref _filterVolunteersIsEmpty, value); }
        }
        #endregion

        #region Popups Properties
        private ChatConnectingPopupViewModel ChatConnectingPopupVM { get;  set; }
        private ChatTermsAndConditionsPopupViewModel ChatTermsAndConditionsPopupVM { get;  set; }
        private ChatLeaseConversationPopupViewModel ChatLeaseConversationPopupVM { get;  set; }
        private ChatRatingPopupViewModel ChatRatingPopupVM { get;  set; }

        #endregion

        #endregion

        #region Commands

        private Command _firstChatCommand;
        public ICommand FirstChatCommand
        {
            get
            {
                return _firstChatCommand ??
                    (_firstChatCommand = new Command(o => OnFirstChatAsync(), o => !IsBusy));
            }
        }

        private async Task OnFirstChatAsync()
        {
            Settings.FirstChat = false;
            NotifyPropertyChanged(nameof(FirstChat));
            await InitializeAsync();
        }


        #region Chat Commands
        private Command _viewCounterpartProfileCommand;
        public Command ViewCounterpartProfileCommand
        {
            get
            {
                return _viewCounterpartProfileCommand ??
                    (_viewCounterpartProfileCommand = new Command(OnCounterpartProfileOpen));
            }
        }

        private async void OnCounterpartProfileOpen(object obj)
        {
            await NavigationService.NavigateToAsync<ChatCounterpartProfileViewModel>(ConversationCounterpart);
        }

        private Command _sendMessageCommand;
        public ICommand SendMessageCommand
        {
            get
            {
                return _sendMessageCommand ??
                    (_sendMessageCommand = new Command(o => OnSendMessageAsync(), o => !IsBusy));
            }
        }

        private Task OnSendMessageAsync()
        {
            return ExecuteOperationAsync(async () =>
            {
                string textToSend = Text;
                Text = string.Empty;

                var newMessage = ChatService.GetMyMessage(textToSend);

                TryToInsertTimeMessage(newMessage, true);
                Messages.Insert(0, newMessage);

                await ChatService.SendMessageAsync(newMessage);
            });
        }


        private Command _leaseConversationPopupCommand;
        public ICommand LeaseConversationPopupCommand
        {
            get
            {
                return _leaseConversationPopupCommand ??
                    (_leaseConversationPopupCommand = new Command(
                        OnLeaseConversationPopup, o => !IsBusy));
            }
        }

        private void OnLeaseConversationPopup(object obj)
        {
            NavigationService.HidePopupAsync();
            NavigationService.ShowPopupAsync(ChatLeaseConversationPopupVM);
        }
        #endregion

        #region Choose volunteer Commands
        private Command _refreshVolunteersCommand;
        public ICommand RefreshVolunteersCommand
        {
            get
            {
                return _refreshVolunteersCommand ??
                    (_refreshVolunteersCommand = new Command(o => OnRefreshVolunteersAsync(o), o => !IsBusy));
            }
        }

        private Task OnRefreshVolunteersAsync(object obj)
        {
            return ExecuteOperationAsync(LoadVolunteersAsync);
        }

        private Command _chooseVolunteerCommand;
        public ICommand ChooseVolunteerCommand
        {
            get
            {
                return _chooseVolunteerCommand ??
                    (_chooseVolunteerCommand = new Command(
                        o => OnChooseVolunteerAsync(o),
                        o => !IsBusy));
            }
        }

        private async Task OnChooseVolunteerAsync(object obj)
        {
            var selectedVolunteer = obj as UserData;
            if (selectedVolunteer == null)
                return;

            if (!Settings.TermsAndConditionsAccept)
            {
                await NavigationService.ShowPopupAsync(ChatTermsAndConditionsPopupVM);
                return;
            }

            try
            {
                //Muestra popup de espera en la conexión
                await NavigationService.ShowPopupAsync(ChatConnectingPopupVM);
                await InitializeChatAsync(selectedVolunteer.PartyId);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
        }
        #endregion

        #region Filter Commands
        private Command _openVolunteersFiltersCommand;
        public ICommand OpenVolunteersFiltersCommand
        {
            get
            {
                return _openVolunteersFiltersCommand ??
                    (_openVolunteersFiltersCommand = new Command(OnVolunteersFiltersOpen));
            }
        }

        async void OnVolunteersFiltersOpen(object obj)
        {
            await NavigationService.NavigateToAsync<ChatFiltersViewModel>();
        }

        private Command _resetVolunteersFilterCommand;
        public ICommand ResetVolunteersFilterCommand
        {
            get
            {
                return _resetVolunteersFilterCommand ??
                    (_resetVolunteersFilterCommand = new Command(OnResetVolunteers));
            }
        }

        void OnResetVolunteers()
        {
            _filters.Reset();
            RefreshVolunters();
        }
        #endregion

        #endregion

        #region Private Methods
        private Task InitializeAsync()
        {
            VolunteerIsActive = ChatService.VolunteerIsActive;

            return ExecuteOperationAsync(async () =>
            {
                await ChatService.InitializeAsync();

                PartyId = ConversationCounterpart?.PartyId;
                if (string.IsNullOrEmpty(PartyId))
                {
                    if (!IsVolunteer)
                    {
                        await LoadVolunteersAsync();
                    }
                }
                else
                {
                    await InitializeChatAsync(PartyId);
                }
            });
        }

        private async Task LoadVolunteersAsync()
        {
            FilterVolunteersIsEmpty = false;
            MessagingCenter.Send(new ToolbarMessage(this["VolunteersListToolbarTitle"]), string.Empty);
            try
            {
                _listVolunteers = await ChatService.GetListVolunteersAsync();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
            RefreshVolunters();
        }

        private async void OnLeaseConversation(object sender, EventArgs e)
        {
            await ExecuteOperationAsync(async () =>
            {
                await NavigationService.HidePopupAsync();
                PartyId = null;
                await ChatService.EndChatAsync();
            },
            finallyAction: async () =>
            {
                if (!IsVolunteer && ChatService.GetConversationMessages().Any())
                {
                    await NavigationService.ShowPopupAsync(ChatRatingPopupVM);
                }
            });
        }

        private void OnChatResetFilters(object sender, EventArgs e)
        {
            OnResetVolunteers();
        }

 


        private void OnChatState(ChatStateMessage obj)
        {
            VolunteerIsActive = obj.VolunteerIsActive;
        }

        private async Task OnChatEventAsync(ChatEventMessage obj)
        {
            if (obj.Type == MessageRouterResultType.Connected)
            {
                PartyId = ConversationCounterpart?.PartyId;

                Messages.Insert(0, new Message
                {
                    DateTime = DateTime.UtcNow,
                    Activity = new Activity()
                    {
                        Text = obj.Message
                    }
                });
                await NavigationService.HidePopupAsync();
            }
            else
            {
                await InitializeAsync();
            }
        }

        private async Task InitializeChatAsync(string partyId)
        {
            Messages.Clear();
          
            if (!ChatService.InConversation)
            {
                await ChatService.InitializeChatAsync(partyId);
            }
            else
            {
                DateTime nextMessageTime = DateTime.MinValue;
                foreach (var message in ChatService.GetConversationMessages())
                {
                    Messages.Add(message);
                    TryToInsertTimeMessage(message);
                }
            }
            MessagingCenter.Send(new ToolbarMessage(ConversationCounterpart.Name), string.Empty);
        }

        private void TryToInsertTimeMessage(Message message, bool firstPosition = false)
        {
            if (_lastMessageTime < message.DateTime)
            {
                if (firstPosition)
                    Messages.Insert(0, new Message() { DateTime = message.DateTime });
                else
                    Messages.Add(new Message() { DateTime = message.DateTime });
                _lastMessageTime = message.DateTime.AddMinutes(MIN_MINUTES_INTEVAL);
            }
        }

        private void OnMesagesReceived(object sender, IList<Message> messages)
        {
            foreach (var message in messages.Reverse())
            {
                TryToInsertTimeMessage(message, true);
                Messages.Insert(0, message);
            }
        }

        private void OnAggregationsReceived(object sender, IList<UserData> newVolunteers)
        {
            _listVolunteers = newVolunteers;
            RefreshVolunters();
        }

        private void RefreshVolunters()
        {
            VolunteersIsEmpty = !_listVolunteers?.Any()?? true;
            CanFilterVolunteers = !VolunteersIsEmpty;

            if (_listVolunteers == null)
                return;

            var volunteersFiltered = _listVolunteers.Where((o =>
                (!o.Age.HasValue || (o.Age < _filters.MaximumAge && o.Age > _filters.MinimumAge)) &&
                (o.Gender == null || o.Gender.StartsWith(_filters.Gender, StringComparison.CurrentCultureIgnoreCase)))).ToList();

            for (int i = 0; i < volunteersFiltered.Count; i++)
            {
                var aggregation = volunteersFiltered[i];
                if (Volunteers.Count > i)
                {
                    if (!aggregation.Equals(Volunteers[i]))
                    {
                        Volunteers.RemoveAt(i);
                        Volunteers.Insert(i, aggregation);
                    }
                }
                else
                {
                    Volunteers.Add(aggregation);
                }
            }

            while (Volunteers.Count != volunteersFiltered.Count)
            {
                Volunteers.RemoveAt(Volunteers.Count - 1);
            }

            FilterVolunteersIsEmpty = !VolunteersIsEmpty && !Volunteers.Any();
        }

        protected override void OnIsBusyChanged()
        {
            if (IsBusy)
                VolunteersIsEmpty = false;

            _chooseVolunteerCommand?.ChangeCanExecute();
            _refreshVolunteersCommand?.ChangeCanExecute();
            _sendMessageCommand?.ChangeCanExecute();
            _openVolunteersFiltersCommand?.ChangeCanExecute();
        }

        #endregion
    }
}
