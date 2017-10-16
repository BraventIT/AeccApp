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
        private readonly IChatService _chatService;

        #region Contructor & Initialize
        public ChatViewModel()
        {
            Messages = new ObservableCollection<Message>();
            Volunteers = new ObservableCollection<Volunteer>();
            ChatFiltersPopupVM = new ChatFiltersPopupViewModel();
            ChatLeaseConversationPopupVM = new ChatLeaseConversationPopupViewModel();
            _chatService = ServiceLocator.Resolve<IChatService>();
        }

        public override Task ActivateAsync()
        {
            MessagingCenter.Subscribe<ChatStateMessage>(this, string.Empty, OnChatState);
            ChatFiltersPopupVM.AppliedFilters += OnChatFiltersAppliedFilters;
            ChatLeaseConversationPopupVM.LeaseChatConversation += OnLeaseConversation;
            _chatService.MessagesReceived += OnMesagesReceived;
            _chatService.AggregationsReceived += OnAggregationsReceived;

            VolunteerIsActive = _chatService.VolunteerIsActive;

            return ExecuteOperationAsync(async () =>
            {
                await _chatService.InitializeAsync();

                PartyId = ConversationCounterpart?.PartyId;
                if (string.IsNullOrEmpty(PartyId))
                {
                    if (!IsVolunteer)
                    {
                        await UpdateVolunteersAsync();
                    }
                }
                else
                {
                    await InitializeChatAsync();
                }
            },
             finallyAction: () => VolunteersIsEmpty = !Volunteers.Any());
        }

        public override void Deactivate()
        {
            MessagingCenter.Unsubscribe<ChatStateMessage>(this, string.Empty);
            ChatFiltersPopupVM.AppliedFilters -= OnChatFiltersAppliedFilters;
            ChatLeaseConversationPopupVM.LeaseChatConversation -= OnLeaseConversation;

            _chatService.MessagesReceived -= OnMesagesReceived;
            _chatService.AggregationsReceived -= OnAggregationsReceived;
        }
        #endregion

        #region Properties
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
            get { return _chatService.ConversationCounterpart; }
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
        public ObservableCollection<Volunteer> Volunteers { get; private set; }

        private bool volunteersIsEmpty;
        public bool VolunteersIsEmpty
        {
            get { return volunteersIsEmpty; }
            set { Set(ref volunteersIsEmpty, value); }
        }
        #endregion

        #region Popups Properties
        public ChatFiltersPopupViewModel ChatFiltersPopupVM { get; private set; }
        public ChatLeaseConversationPopupViewModel ChatLeaseConversationPopupVM { get; private set; }

        private bool _isVolunteerProfileVisible;
        public bool IsVolunteerProfileVisible
        {
            get { return _isVolunteerProfileVisible; }
            set { Set(ref _isVolunteerProfileVisible, value); }
        }

        private bool isLeaseConversationPopupVisible;
        public bool IsLeaseConversationPopupVisible
        {
            get { return isLeaseConversationPopupVisible; }
            set { Set(ref isLeaseConversationPopupVisible, value); }
        }

        private bool _ratingPopupVisible;
        public bool RatingPopupVisible
        {
            get { return _ratingPopupVisible; }
            set { Set(ref _ratingPopupVisible, value); }
        }


        #endregion

        #endregion

        #region Commands

        #region Chat Commands

        private Command _ratingCommand;
        public ICommand RatingCommand
        {
            get
            {
                return _ratingCommand ??
                    (_ratingCommand = new Command(OnRatingCommand, (o) => !IsBusy));
            }
        }

        private async void OnRatingCommand(object obj)
        {
            //TODO Manage rating result:
            switch (obj)
            {
                case "1":
                    var test1 = "1 estrella";
                    break;

                case "2":
                    var test2 = "2 estrellas";

                    break;

                case "3":
                    var test3 = "3 estrellas";

                    break;

                case "4":
                    var test4 = "4 estrellas";

                    break;

                case "5":
                    var test = "5 estrellas";

                    break;

                default:
                    break;
            }
            RatingPopupVisible = false;
            await ExecuteOperationAsync(() => NavigationService.NavigateToAsync<DashboardViewModel>());
        }

        private Command _viewVolunteerProfileCommand;
        public ICommand ViewVolunteerProfileCommand
        {
            get
            {
                return _viewVolunteerProfileCommand ??
                    (_viewVolunteerProfileCommand = new Command(OnVolunteerProfileOpen, (o) => !IsBusy));
            }
        }

        private void OnVolunteerProfileOpen(object obj)
        {
            isLeaseConversationPopupVisible = false;
            IsVolunteerProfileVisible = !IsVolunteerProfileVisible;
        }

        private Command sendMessageCommand;
        public ICommand SendMessageCommand
        {
            get
            {
                return sendMessageCommand ??
                    (sendMessageCommand = new Command(o => OnSendMessageAsync(), (o) => !IsBusy));
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
                await _chatService.SendMessageAsync(textToSend);
            });
        }

       

        private async void OnLeaseConversation(object sender, EventArgs e)
        {
            await ExecuteOperationAsync(() => _chatService.EndChatAsync());
            await NavigationService.HidePopupAsync();
            if (!IsVolunteer)
            {
                RatingPopupVisible = true;
            }
        }
  
        private Command _leaseConversationPopupCommand;
        public ICommand LeaseConversationPopupCommand
        {
            get
            {
                return _leaseConversationPopupCommand ??
                    (_leaseConversationPopupCommand = new Command(
                        OnLeaseConversationPopup, (o) => !IsBusy));
            }
        }

        private void OnLeaseConversationPopup(object obj)
        {
            NavigationService.HidePopupAsync();
            NavigationService.ShowPopupAsync(ChatLeaseConversationPopupVM);
        }
        #endregion

        #region Choose volunteer Commands
        private Command refreshVolunteersCommand;
        public ICommand RefreshVolunteersCommand
        {
            get
            {
                return refreshVolunteersCommand ??
                    (refreshVolunteersCommand = new Command((o) => OnRefreshVolunteersAsync(o), (o) => !IsBusy));
            }
        }

        private Task OnRefreshVolunteersAsync(object obj)
        {
            return ExecuteOperationAsync(async () =>
            {
                Volunteers.Clear();
                Volunteers.AddRange(await _chatService.GetListVolunteersAsync());
            },
            finallyAction: () =>
                 VolunteersIsEmpty = !Volunteers.Any());
        }

        private Command chooseVolunteerCommand;
        public ICommand ChooseVolunteerCommand
        {
            get
            {
                return chooseVolunteerCommand ??
                    (chooseVolunteerCommand = new Command(
                        (o) => OnChooseVolunteerAsync(o),
                        (o) => !IsBusy));
            }
        }

        private Task OnChooseVolunteerAsync(object obj)
        {
            return ExecuteOperationAsync(async () =>
            {
                var selectedVolunteer = obj as Volunteer;
                PartyId = selectedVolunteer.PartyId;
                await InitializeChatAsync();
            });
        }
        #endregion

        private Command _openFiltersCommand;
        public ICommand OpenFiltersCommand
        {
            get
            {
                return _openFiltersCommand ??
                    (_openFiltersCommand = new Command(OnFiltersOpenHandler, (o) => !IsBusy));
            }
        }

        void OnFiltersOpenHandler(object obj)
        {
            isLeaseConversationPopupVisible = false;
            IsVolunteerProfileVisible = false;
            NavigationService.ShowPopupAsync(ChatFiltersPopupVM);
        }

        #endregion

        #region Private Methods
        private async Task UpdateVolunteersAsync()
        {
            Volunteers.Clear();
            var listAggregations = await _chatService.GetListVolunteersAsync();
            if (listAggregations != null)
                Volunteers.AddRange(listAggregations);
        }

        private async void OnChatFiltersAppliedFilters(object sender, EventArgs e)
        {
            await ExecuteOperationAsync(async () =>
            {
                await NavigationService.HidePopupAsync();
                if (ChatFiltersPopupVM.MinimumAge > ChatFiltersPopupVM.MaximumAge)
                {
                    //Temporary sliders (Not range sliders yet) not following filters logic
                    return;
                }
                else
                {
                    Volunteers.Clear();
                    Volunteers.AddRange(await _chatService.GetListVolunteersAsync());
                    foreach (var item in Volunteers)
                    {
                        int volunteerAge = 0;
                        if (Int32.TryParse(item.Age, out volunteerAge))
                        {
                            if (volunteerAge > ChatFiltersPopupVM.MaximumAge || volunteerAge < ChatFiltersPopupVM.MinimumAge)
                            {
                                Volunteers.Remove(item);
                            }
                        }
                    }
                }
            });
        }

        public override bool OnBackButtonPressed()
        {
            bool returnValue = false;
            if (RatingPopupVisible)
            {
                RatingPopupVisible = false;
                returnValue = true;
            }
            else if (IsLeaseConversationPopupVisible)
            {
                IsLeaseConversationPopupVisible = false;
                returnValue = true;
            }
            else if (IsVolunteerProfileVisible)
            {
                IsVolunteerProfileVisible = false;
                returnValue = true;
            }

            return returnValue;
        }

        private void OnChatState(ChatStateMessage obj)
        {
            VolunteerIsActive = obj.VolunteerIsActive;
        }

        private async Task InitializeChatAsync()
        {
            Messages.Clear();
            if (!_chatService.InConversation)
            {
                await _chatService.InitializeChatAsync(_partyId);
                NotifyPropertyChanged(nameof(ConversationCounterpart));
            }
            else
            {
                Messages.AddRange(_chatService.GetConversationMessages());
            }
        }

        private void OnMesagesReceived(object sender, IList<Message> messages)
        {
            foreach (var message in messages.Reverse())
            {
                Messages.Insert(0, message);
            }
        }



        private void OnAggregationsReceived(object sender, IList<Volunteer> newVolunteers)
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

            chooseVolunteerCommand?.ChangeCanExecute();
            refreshVolunteersCommand?.ChangeCanExecute();
            sendMessageCommand?.ChangeCanExecute();
        }

        #endregion
    }
}
