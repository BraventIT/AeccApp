using AeccApp.Core.Messages;
using AeccApp.Core.Models;
using AeccBot.MessageRouting;
using AeccBot.Models;
using Microsoft.Bot.Connector.DirectLine;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Forms;

/// <summary>
/// TERMINOLOGY
/// --------------------------------------------------------------------------------------------------------------------------------
/// Aggregation (channel)   A channel where the chat requests are sent (when a volunteer is available).
///                         The volunteer in the aggregation channel can accept the requests.
/// Engagement (channel)    Is created when a request is accepted - the acceptor and the one accepted form a connection 
///                         (1:1 chat where the bot relays the messages between the users).
/// Party                   A user/bot in a specific conversation.
/// Activity                Is the basic communication type for the Bot Framework.
/// </summary>

namespace AeccApp.Core.Services
{
    public class ChatService : BaseNotifyProperty, IChatService
    {
        private const int TIMEOUT_MS =
#if DEBUG
            10000;
#else
            30000;
#endif

        private const int POOLING_MS = 750;
        private const int AGGREGATION_POOLING_CNT = (15 * 1000) / POOLING_MS; // 15 sg
        private double _pooling_cnt;

        /// <summary>
        /// DirectLine client
        /// </summary>
        private DirectLineClient _client = null;
        private Conversation _mainConversation;
        private ChannelAccountWithUserData _account;

        /// <summary>
        /// Is sending a message
        /// </summary>
        private bool _sendingMessage;

        private IList<Volunteer> _aggregations;
        private IList<Message> _conversationMessages;

        private Party _conversationCounterpartParty;

        /// <summary>
        /// Task completions for operations
        /// </summary>
        private TaskCompletionSource<bool> _listAggregationsResponseTask;
        private TaskCompletionSource<bool> _initiateEngagementTask;
        private TaskCompletionSource<bool> _endEngagementTask;
        private TaskCompletionSource<bool> _setVolunteerStateTask;
        private TaskCompletionSource<bool> _acceptAndRejectRequestTask;
        private TaskCompletionSource<string> _engagementCounterpartTask;

        private SemaphoreSlim _initializeLock;

        public event EventHandler<IList<Message>> MessagesReceived;
        public event EventHandler<IList<Volunteer>> AggregationsReceived;

        private bool _inConversation;
        public bool InConversation
        {
            get { return _inConversation; }
            set
            {
                if (!value)
                {
                    _conversationCounterpartParty = null;
                }
                if (_inConversation != value)
                {
                    _inConversation = value;

                    MessagingCenter.Send(new ChatStateMessage(VolunteerIsActive, _inConversation), string.Empty);
                }
            }
        }

        private bool _volunteerIsActive;
        public bool VolunteerIsActive
        {
            get { return _volunteerIsActive; }
            set
            {
                if (_volunteerIsActive != value)
                {
                    _volunteerIsActive = value;
                    MessagingCenter.Send(new ChatStateMessage(_volunteerIsActive, InConversation), string.Empty);
                }
            }
        }

        public UserData ConversationCounterpart
        {
            get { return new UserData(_conversationCounterpartParty); }
        }

        private GlobalSetting GSetting { get { return GlobalSetting.Instance; } }

        #region Contructor & Initialize
        public ChatService()
        {
            _conversationMessages = new List<Message>();
            _initializeLock = new SemaphoreSlim(1);
        }

        public async Task InitializeAsync()
        {
            try
            {
                await _initializeLock.WaitAsync();

                if (_mainConversation == null)
                {
                    _client = new DirectLineClient(GSetting.AeccBotSecret);
                    _mainConversation = await _client.Conversations.StartConversationAsync();
                    _account = new ChannelAccountWithUserData(GSetting.User.UserId, GSetting.User.UserName)
                    {
                        FirstName = GSetting.User.FirstName,
                        Surname = GSetting.User.Surname,
                        Email = GSetting.User.Email
                    };
                    ListenToBotMessages();
                    await TryToFillConversationCounterpartAsync();
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
            finally
            {
                _initializeLock.Release();
            }
        }
        #endregion

        #region Public Methods
        public async Task SendMessageAsync(string messageText)
        {
            var ct = new CancellationTokenSource(TIMEOUT_MS);
            await SendActivity(messageText, type: ActivityTypes.Message, token: ct.Token);

            _conversationMessages.Insert(0, new Message
            {
                DateTime = DateTime.UtcNow,
                Activity = new Activity() { Text = messageText, From= _account }
            });
        }

        public async Task<IList<Volunteer>> GetListVolunteersAsync()
        {
            var ct = new CancellationTokenSource(TIMEOUT_MS);
            await SendActivity(BackChannelCommands.CommandListAggregations, token: ct.Token);

            if (!(_aggregations?.Any() ?? false))
            {
                _listAggregationsResponseTask = new TaskCompletionSource<bool>();
                ct.Token.Register(() => _listAggregationsResponseTask?.TrySetCanceled(), false);

                await _listAggregationsResponseTask.Task;
                _listAggregationsResponseTask = null;
            }
            return _aggregations;
        }

        public async Task<bool> InitializeChatAsync(string partyId)
        {
            try
            {
                var ct = new CancellationTokenSource(TIMEOUT_MS);
                await SendActivity(BackChannelCommands.CommandInitiateEngagement, data: partyId, token: ct.Token);

                _conversationMessages.Clear();
                _initiateEngagementTask = new TaskCompletionSource<bool>();
                ct.Token.Register(() => _initiateEngagementTask?.TrySetCanceled(), false);

                _conversationCounterpartParty = Party.FromJsonString(partyId);
                bool result = await _initiateEngagementTask?.Task;
                return result;
            }
            catch (Exception)
            {
                InConversation = false;
                throw;
            }
        }

        public async Task<bool> EndChatAsync()
        {
            var ct = new CancellationTokenSource(TIMEOUT_MS);
            await SendActivity(BackChannelCommands.CommandEndEngagement, token: ct.Token);

            _endEngagementTask = new TaskCompletionSource<bool>();
            ct.Token.Register(() => _endEngagementTask?.TrySetCanceled(), false);

            bool result = await _endEngagementTask.Task;

            InConversation = false;

            return result;
        }

        public IList<Message> GetConversationMessages()
        {
            return _conversationMessages;
        }

        #region Only Volunteers Methods
        public async Task<bool> SetVolunteerState(bool isActive)
        {
            if (VolunteerIsActive == isActive)
                return true;

            string command = isActive ?
                BackChannelCommands.CommandAddAggregation :
                BackChannelCommands.CommandDeleteAggregation;

            var ct = new CancellationTokenSource(TIMEOUT_MS);
            await SendActivity(command, token: ct.Token);

            _setVolunteerStateTask = new TaskCompletionSource<bool>();
            ct.Token.Register(() => _setVolunteerStateTask?.TrySetCanceled(), false);

            bool result = await _setVolunteerStateTask?.Task;

            if (result)
            {
                InConversation = false;
                VolunteerIsActive = isActive;
            }
            return result;
        }

        public async Task<bool> AcceptAndRejectRequestAsync(bool doAccept, string partyId)
        {
            string command = doAccept ?
                BackChannelCommands.CommandAcceptRequest : BackChannelCommands.CommandRejectRequest;

            var ct = new CancellationTokenSource(TIMEOUT_MS);
            await SendActivity(command, data: partyId, token: ct.Token);

            _conversationMessages.Clear();
            _acceptAndRejectRequestTask = new TaskCompletionSource<bool>();
            ct.Token.Register(() => _acceptAndRejectRequestTask?.TrySetCanceled(), false);

            _conversationCounterpartParty = Party.FromJsonString(partyId);
            bool result = await _acceptAndRejectRequestTask.Task;
            return result;
        }
        #endregion

        #endregion

        #region Private Methods
        /// <summary>
        /// Ask if I´m in conversation
        /// </summary>
        private async Task TryToFillConversationCounterpartAsync()
        {
            try
            {
                var ct = new CancellationTokenSource(TIMEOUT_MS);
                await SendActivity(BackChannelCommands.CommandEngagementCounterpart, token: ct.Token);

                _engagementCounterpartTask = new TaskCompletionSource<string>();
                ct.Token.Register(() => _engagementCounterpartTask?.TrySetCanceled(), false);

                string result = await _engagementCounterpartTask?.Task;
                if (!string.IsNullOrEmpty(result))
                {
                    _conversationCounterpartParty = Party.FromJsonString(result);
                    if (_conversationCounterpartParty != null)
                    {
                        InConversation = true;
                        await SendActivity(BackChannelCommands.CommandSyncEngagement);
                    }
                }
            }
            catch (Exception)
            {
                InConversation = false;
            }
        }

        /// <summary>
        /// Send activity to bot
        /// </summary>
        /// <param name="text">text to send</param>
        /// <param name="type">type (Message or Event)</param>
        /// <param name="data">data to send in event activity</param>
        /// <param name="token">cancellation token context</param>
        /// <returns></returns>
        private async Task SendActivity(string text, string type = ActivityTypes.Event, object data = null, CancellationToken token = default(CancellationToken))
        {
            try
            {
                Activity activity = new Activity
                {
                    From = _account,
                    Text = (type != ActivityTypes.Event) ?
                        text : $"{BackChannelCommands.BackChannelKeyword} {text}",
                    Type = type,
                    ChannelData = data
                };

                _sendingMessage = true;
                await _client.Conversations.PostActivityAsync(_mainConversation.ConversationId, activity, token);
            }
            finally
            {
                _sendingMessage = false;
            }
        }

        /// <summary>
        /// Endless method to listen bot messages
        /// </summary>
        private async void ListenToBotMessages()
        {
            string watermark = null;

            while (true)
            {
                try
                {
                    var activitySet = await _client.Conversations.GetActivitiesAsync(_mainConversation.ConversationId, watermark);
                    // only new messages required
                    watermark = activitySet?.Watermark;

                    List<Message> newMessages = new List<Message>();
                    foreach (var activity in activitySet.Activities)
                    {
                        // oneselft messages discarded
                        if (activity.From.Id != _account.Id)
                        {
                            switch (activity.Type)
                            {
                                case ActivityTypes.Message:
                                    newMessages.Add(ProcessMessage(activity)); break;
                                case ActivityTypes.Event:
                                   await ProcessEventResulAsync(activity); break;
                                default:
                                    break;
                            }
                        }
                    }

                    if (newMessages.Any())
                    {
                        foreach (var message in newMessages)
                        {
                            _conversationMessages.Insert(0, message);
                        }

                        MessagesReceived?.Invoke(this, newMessages);
                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex);
                }
                finally
                {
                    do
                    {
                        await Task.Delay(POOLING_MS);
                        _pooling_cnt++;
                        if (VolunteerIsActive && _pooling_cnt % AGGREGATION_POOLING_CNT == 0)
                        {
                            await SendActivity(BackChannelCommands.CommandCheckAggregation);
                        }
                    }
                    // Wait while is sending message
                    while (_sendingMessage);
                }
            }
        }

        #region ProcessEvent Result
        /// <summary>
        /// Process event activity response
        /// </summary>
        /// <param name="activity">activity to process</param>
        private async Task ProcessEventResulAsync(Activity activity)
        {
            if (string.IsNullOrEmpty(activity.Text) || !activity.Text.StartsWith($"{BackChannelCommands.BackChannelKeyword} "))
                return;

            // remove backchannel prefix
            var messagePart = activity.Text.Remove(0, BackChannelCommands.BackChannelKeyword.Length + 1).Split(' ');
            if (!messagePart.Any())
                return;

            switch (messagePart.First())
            {
                case BackChannelCommands.CommandEngagementCounterpart:
                    HandleCommandEngagementCounterpart(activity); break;
                case BackChannelCommands.CommandAddAggregation:
                    HandleCommandAggregation(activity, true); break;
                case BackChannelCommands.CommandDeleteAggregation:
                    HandleCommandAggregation(activity, false); break;
                case BackChannelCommands.CommandCheckAggregation:
                    HandleCommandCheckAggregation(activity); break;
                case BackChannelCommands.CommandAcceptRequest:
                    HandleCommandRequest(activity, true); break;
                case BackChannelCommands.CommandRejectRequest:
                    HandleCommandRequest(activity, false); break;
                case BackChannelCommands.CommandListAggregations:
                    HandleCommandListAggregations(activity); break;
                case BackChannelCommands.CommandInitiateEngagement:
                    HandleCommandInitiateEngagement(activity); break;
                case BackChannelCommands.CommandEndEngagement:
                    HandleCommandEndEngagement(activity); break;
                case BackChannelCommands.CommandSyncEngagement:
                    await HandleCommandSyncEngagementAsync(activity); break;
                case BackChannelCommands.MessageRouter:
                    if (messagePart.Length == 2)
                    {
                        MessageRouterResultType type;
                        Enum.TryParse(messagePart[1], out type);
                        HandleMessageRouter(activity, type);
                    }
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// Handle engagement counterpart
        /// </summary>
        /// <param name="activity">Activity to process</param>
        private void HandleCommandEngagementCounterpart(Activity activity)
        {
            string result = string.Empty;

            if (activity.ChannelData != null) // Data required
            {
                result = activity.ChannelData.ToString();
            }
            _engagementCounterpartTask?.SetResult(result);
        }

        /// <summary>
        /// Handle aggregation commands response
        /// </summary>
        /// <param name="activity">Activity to process</param>
        /// <param name="isAdded">Added or deleted aggregation (Volunteer online or offline)</param>
        private void HandleCommandAggregation(Activity activity, bool isAdded)
        {
            bool result = false;

            if (activity.ChannelData != null) // Data required
            {
                if (!bool.TryParse(activity.ChannelData.ToString(), out result))
                {
                    var type = isAdded ?
                        MessageRouterResultType.AddAggregation
                        : MessageRouterResultType.DeleteAggregation;
                    // Send ChatEvent message
                    MessagingCenter.Send(new ChatEventMessage(type, activity.ChannelData.ToString()), string.Empty);
                }
            }
            _setVolunteerStateTask?.SetResult(result);
        }

        private void HandleCommandCheckAggregation(Activity activity)
        {
            bool result = false;
            if (activity.ChannelData != null)
            {
                result = activity.ChannelData is bool;
            }

            VolunteerIsActive = result;
        }

        /// <summary>
        /// Handler conversation request command response
        /// </summary>
        /// <param name="activity">Activity to process</param>
        /// <param name="doAccept">Accept or reject request (volunteer decide about beneficiary conversation request)</param>
        private void HandleCommandRequest(Activity activity, bool doAccept)
        {
            bool result = false;

            if (activity.ChannelData != null) // Data required
            {
                if (!bool.TryParse(activity.ChannelData.ToString(), out result))
                {
                    var type = doAccept ?
                        MessageRouterResultType.Connected
                        : MessageRouterResultType.ConnectionRejected;
                    // Send ChatEvent message
                    MessagingCenter.Send(new ChatEventMessage(type, activity.ChannelData.ToString()), string.Empty);
                }

                InConversation = (result) ? doAccept : false;
            }
            _acceptAndRejectRequestTask?.SetResult(result);
        }

        /// <summary>
        /// Handler volunteers list response
        /// </summary>
        /// <param name="activity">Activity to process</param>
        private void HandleCommandListAggregations(Activity activity)
        {
            var aggregationListBulk = activity.ChannelData as JArray;
            var aggregationsRaw = aggregationListBulk?.ToObject<List<Party>>();

            _aggregations = aggregationsRaw.Select(o => new Volunteer(o)).ToList();

            if (_listAggregationsResponseTask != null)
            {
                _listAggregationsResponseTask.SetResult(true);
            }
            else
            {
                AggregationsReceived?.Invoke(this, _aggregations);
            }
        }

        /// <summary>
        /// Handler global message router
        /// </summary>
        /// <param name="activity">Activity to process</param>
        /// <param name="type">Message router type</param>
        private void HandleMessageRouter(Activity activity, MessageRouterResultType type)
        {
            switch (type)
            {
                // conversation request received
                // Data -> PartyId from requested user
                case MessageRouterResultType.ConnectionRequested:
                    MessagingCenter.Send(new ChatEngagementEventMessage(activity.ChannelData.ToString()), string.Empty);
                    break;
                default:
                    // conversation reject o removed
                    if (type == MessageRouterResultType.ConnectionRejected || type == MessageRouterResultType.Disconnected)
                    {
                        InConversation = false;
                        if (GSetting.User.IsVolunteer)
                        {
                            VolunteerIsActive = true;
                        }
                    }
                    MessagingCenter.Send(new ChatEventMessage(type, activity.ChannelData.ToString()), string.Empty);
                    break;
            }
        }

        /// <summary>
        /// Handler initiate engagement response (init conversation beween users)
        /// </summary>
        /// <param name="activity">Activity to process</param>
        private void HandleCommandInitiateEngagement(Activity activity)
        {
            if (_initiateEngagementTask != null)
            {
                bool result = false;
                if (activity.ChannelData != null)
                {
                    result = activity.ChannelData is bool;
                }

                InConversation = result;
                
                _initiateEngagementTask.SetResult(result);
            }
        }

        /// <summary>
        /// Handler end engagement response (end conversation)
        /// </summary>
        /// <param name="activity">Activity to process</param>
        private void HandleCommandEndEngagement(Activity activity)
        {
            if (_endEngagementTask != null)
            {
                bool result = false;
                if (activity.ChannelData != null)
                {
                    result = activity.ChannelData is bool;
                }
                _endEngagementTask.SetResult(result);
            }
        }

        private async Task HandleCommandSyncEngagementAsync(Activity activity)
        {
            if (activity.ChannelData != null)
            {
                var messagesBulk = activity.ChannelData as JArray;
                IList<Message> messages = messagesBulk?.ToObject<List<Message>>();
                if (messages != null)
                {
                    _conversationMessages = messages;
                    InConversation = true;
                    MessagesReceived?.Invoke(this, _conversationMessages);
                }
            }
            else
            {

                await SendActivity(BackChannelCommands.CommandSyncEngagement, data: _conversationMessages);
            }
        }
        #endregion

        /// <summary>
        /// Generate message from received activity
        /// </summary>
        /// <param name="activity">activity</param>
        /// <returns></returns>
        private Message ProcessMessage(Activity activity)
        {
            // Removing extra * we don't want to see in the output
            activity.Text = activity.Text.Replace("\r\n*", "\r\n");

            var message = new Message
            {
                DateTime = activity.Timestamp.Value,
                Activity = activity
            };

            message.Activity.From = _conversationCounterpartParty.ChannelAccount;
            InConversation = true;

            // Send message with the new message received
            MessagingCenter.Send(new ChatMessageReceivedMessage(message), string.Empty);
            return message;
        }

        #endregion
    }
}

