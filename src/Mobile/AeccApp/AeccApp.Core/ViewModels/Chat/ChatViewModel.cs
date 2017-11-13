using AeccApp.Core.Extensions;
using AeccApp.Core.Messages;
using AeccApp.Core.Models;
using AeccApp.Core.Services;
using AeccApp.Core.ViewModels.Popups;
using Microsoft.Bot.Connector.DirectLine;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace AeccApp.Core.ViewModels
{
    public class ChatViewModel : ViewModelBase
    {
        private IChatService ChatService { get; } = ServiceLocator.ChatService;
        private IList<UserData> _listVolunteers;

        #region Contructor & Initialize
        public ChatViewModel()
        {
            Messages = new ObservableCollection<Message>();
            Volunteers = new ObservableCollection<UserData>();
            ChatFiltersPopupVM = new ChatFiltersPopupViewModel();
            ChatRatingPopupVM = new ChatRatingPopupViewModel();
            ChatCounterpartProfilePopupVM = new ChatCounterpartProfilePopupViewModel();
            ChatLeaseConversationPopupVM = new ChatLeaseConversationPopupViewModel();
            ChatTermsAndConditionsPopupVM = new ChatTermsAndConditionsPopupViewModel();
            ChatConnectingPopupVM = new ChatConnectingPopupViewModel();
        }

        public override Task ActivateAsync()
        {
            if (IsVolunteer == false && Settings.TermsAndConditionsAccept == false )
            {
               return NavigationService.ShowPopupAsync(ChatTermsAndConditionsPopupVM);
            }
         

            MessagingCenter.Subscribe<ChatStateMessage>(this, string.Empty, OnChatState);
            ChatFiltersPopupVM.AppliedFilters += OnChatAppliedFilters;
            ChatLeaseConversationPopupVM.LeaseChatConversation += OnLeaseConversation;
            ChatService.MessagesReceived += OnMesagesReceived;
            ChatService.AggregationsReceived += OnAggregationsReceived;

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
                    await InitializeChatAsync();
                }
            });
        }

        public override void Deactivate()
        {
            MessagingCenter.Unsubscribe<ChatStateMessage>(this, string.Empty);
            ChatFiltersPopupVM.AppliedFilters -= OnChatAppliedFilters;
            ChatLeaseConversationPopupVM.LeaseChatConversation -= OnLeaseConversation;
            ChatService.MessagesReceived -= OnMesagesReceived;
            ChatService.AggregationsReceived -= OnAggregationsReceived;
        }
        #endregion

        #region Properties
        public ChatConnectingPopupViewModel ChatConnectingPopupVM { get; set; }

        private string _partyId;
        public string PartyId
        {
            get { return _partyId; }
            set
            {
                if (Set(ref _partyId, value))
                {
                    NotifyPropertyChanged(nameof(ConversationCounterpart));
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
        public ChatTermsAndConditionsPopupViewModel ChatTermsAndConditionsPopupVM { get; private set; }
        public ChatFiltersPopupViewModel ChatFiltersPopupVM { get; private set; }
        public ChatLeaseConversationPopupViewModel ChatLeaseConversationPopupVM { get; private set; }
        public ChatRatingPopupViewModel ChatRatingPopupVM { get; private set; }
        public ChatCounterpartProfilePopupViewModel ChatCounterpartProfilePopupVM { get; private set; }


        #endregion

        #endregion

        #region Commands

        #region Chat Commands
        private Command _viewVolunteerProfileCommand;
        public ICommand ViewVolunteerProfileCommand
        {
            get
            {
                return _viewVolunteerProfileCommand ??
                    (_viewVolunteerProfileCommand = new Command(OnVolunteerProfileOpen, o => !IsBusy));
            }
        }

        private async void OnVolunteerProfileOpen(object obj)
        {
            await NavigationService.HidePopupAsync();
            await NavigationService.ShowPopupAsync(ChatCounterpartProfilePopupVM);
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

                Messages.Insert(0, new Message
                {
                    DateTime = DateTime.UtcNow,
                    Activity = new Activity()
                    {
                        Text = textToSend
                    }
                });
                await ChatService.SendMessageAsync(textToSend);
            });
        }

        private async void OnLeaseConversation(object sender, EventArgs e)
        {
            await ExecuteOperationAsync(() => ChatService.EndChatAsync());
            await NavigationService.HidePopupAsync();
            if (!IsVolunteer)
            {
                await NavigationService.ShowPopupAsync(ChatRatingPopupVM);
            }
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

        private Task OnChooseVolunteerAsync(object obj)
        {
            return ExecuteOperationAsync(async () =>
            {
                var selectedVolunteer = obj as UserData;
                PartyId = selectedVolunteer.PartyId;
                //Muestra popup de espera en la conexión
                NavigationService.ShowPopupAsync(ChatConnectingPopupVM);
                await InitializeChatAsync();
            });
        }
        #endregion

        private Command _openVolunteersFiltersCommand;
        public ICommand OpenVolunteersFiltersCommand
        {
            get
            {
                return _openVolunteersFiltersCommand ??
                    (_openVolunteersFiltersCommand = new Command(OnVolunteersFiltersOpen, o => !IsBusy));
            }
        }

        void OnVolunteersFiltersOpen(object obj)
        {
            NavigationService.HidePopupAsync();
            NavigationService.ShowPopupAsync(ChatFiltersPopupVM);
        }

        private Command _resetVolunteersFilterCommand;
        public ICommand ResetVolunteersFilterCommand
        {
            get
            {
                return _resetVolunteersFilterCommand ??
                    (_resetVolunteersFilterCommand = new Command(o => ChatFiltersPopupVM.Reset()));
            }
        }
        #endregion

        #region Private Methods
        private async Task LoadVolunteersAsync()
        {
            try
            {
                Volunteers.Clear();
                FilterVolunteersIsEmpty = false;
                
                _listVolunteers = await ChatService.GetListVolunteersAsync();

                FillVolunters();  
            }
            finally
            {
                VolunteersIsEmpty = !Volunteers.Any();
                CanFilterVolunteers = !VolunteersIsEmpty;
            }
        }

        private async void OnChatAppliedFilters(object sender, EventArgs e)
        {
            await ExecuteOperationAsync(async () =>
            {
                Volunteers.Clear();
                await NavigationService.HidePopupAsync();

                FillVolunters();
            });
        }

        private void FillVolunters()
        {
            foreach (var item in _listVolunteers)
            {
                if (item.Age < ChatFiltersPopupVM.MaximumAge && item.Age > ChatFiltersPopupVM.MinimumAge)
                {
                    Volunteers.Add(item);
                }
            }

            FilterVolunteersIsEmpty = !Volunteers.Any();
        }

        private void OnChatState(ChatStateMessage obj)
        {
            VolunteerIsActive = obj.VolunteerIsActive;
        }

        private async Task InitializeChatAsync()
        {
            Messages.Clear();
            if (!ChatService.InConversation)
            {
                await ChatService.InitializeChatAsync(_partyId);
                NotifyPropertyChanged(nameof(ConversationCounterpart));
                ChatCounterpartProfilePopupVM.Counterpart = ChatService.ConversationCounterpart;

            }
            else
            {
                Messages.AddRange(ChatService.GetConversationMessages());
            }
        }

        private void OnMesagesReceived(object sender, IList<Message> messages)
        {
            foreach (var message in messages.Reverse())
            {
                if (message.Activity.Text.StartsWith("ahora estás hablando con"))
                {
                    //Oculta popup de espera en la conexión //TODO Revisar cuando llegue codigo unico de mensaje
                    NavigationService.HidePopupAsync();
                }
                Messages.Insert(0, message);
            }
        }

        private void OnAggregationsReceived(object sender, IList<UserData> newVolunteers)
        {
            for (int i = 0; i < newVolunteers.Count; i++)
            {
                var aggregation = newVolunteers[i];
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

            while (Volunteers.Count != newVolunteers.Count)
            {
                Volunteers.RemoveAt(Volunteers.Count - 1);
            }
            VolunteersIsEmpty = !Volunteers.Any();
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
