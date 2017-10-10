using AeccBot.CommandHandling;
using AeccBot.MessageRouting.DataStore;
using AeccBot.Models;
using AeccBot.Utils;
using Microsoft.Bot.Connector;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;

namespace AeccBot.MessageRouting
{
    public class MessageRouterResultHandler : IMessageRouterResultHandler
    {
        private MessageRouterManager MessageRouterManager
        {
            get { return WebApiConfig.MessageRouterManager; }
        }

        private IRoutingDataManager RoutingDataManager
        {
            get { return WebApiConfig.RoutingDataManager; }
        }

        /// <summary>
        /// From IMessageRouterResultHandler.
        /// </summary>
        /// <param name="messageRouterResult">The result to handle.</param>
        /// <returns></returns>
        public virtual async Task HandleResultAsync(MessageRouterResult messageRouterResult)
        {
            if (messageRouterResult == null)
            {
                throw new ArgumentNullException($"The given result ({nameof(messageRouterResult)}) is null");
            }

#if DEBUG
            RoutingDataManager.AddMessageRouterResult(messageRouterResult);
#endif

            switch (messageRouterResult.Type)
            {
                case MessageRouterResultType.NoActionTaken:
                case MessageRouterResultType.OK:
                    // No need to do anything
                    break;
                case MessageRouterResultType.ConnectionRequested:
                case MessageRouterResultType.ConnectionAlreadyRequested:
                case MessageRouterResultType.ConnectionRejected:
                case MessageRouterResultType.Connected:
                case MessageRouterResultType.Disconnected:
                    await HandleConnectionChangedResultAsync(messageRouterResult);
                    break;
                case MessageRouterResultType.NoAgentsAvailable:
                    await HandleNoAgentsAvailableResultAsync(messageRouterResult);
                    break;
                case MessageRouterResultType.NoAggregationChannel:
                    await HandleNoAggregationChannelResultAsync(messageRouterResult);
                    break;
                case MessageRouterResultType.FailedToForwardMessage:
                    await HandleFailedToForwardMessageAsync(messageRouterResult);
                    break;
                case MessageRouterResultType.Error:
                    await HandleErrorAsync(messageRouterResult);
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// Notifies the conversation client (customer) or owner (agent) that an error has occured and
        /// sends the error message
        /// </summary>
        /// <param name="messageRouterResult">The result to handle.</param>
        private async Task HandleErrorAsync(MessageRouterResult messageRouterResult)
        {
            if (string.IsNullOrEmpty(messageRouterResult.ErrorMessage))
            {
                Debug.WriteLine("An error occured");
            }
            else
            {
                IList<Party> aggregationParties = RoutingDataManager.GetAggregationParties();

                if (aggregationParties == null || aggregationParties.Count == 0)
                {
                    if (messageRouterResult.ConversationOwnerParty != null)
                    {
                        string eventText = (messageRouterResult.ConversationOwnerParty?.AllowBackchannel ?? false) ?
                            BackChannelCommands.GetMessageRouter(MessageRouterResultType.Error)
                            : string.Empty;

                        await MessageRouterManager.SendMessageToPartyByBotAsync(
                            messageRouterResult.ConversationOwnerParty, messageRouterResult.ErrorMessage, eventText);
                    }
                }
                else
                {
                    foreach (Party aggregationChannel in aggregationParties)
                    {
                        string eventText = (aggregationChannel?.AllowBackchannel ?? false) ?
                           BackChannelCommands.GetMessageRouter(MessageRouterResultType.Error)
                           : string.Empty;

                        await MessageRouterManager.SendMessageToPartyByBotAsync(
                            aggregationChannel, messageRouterResult.ErrorMessage, eventText);
                    }
                }

                Debug.WriteLine(messageRouterResult.ErrorMessage);
            }

        }

        /// <summary>
        /// Notifies the conversation client (customer) or the conversation owner (agent) that there 
        /// was a problem forwarding their message
        /// </summary>
        /// <param name="messageRouterResult">The result to handle.</param>
        private  async Task HandleFailedToForwardMessageAsync(MessageRouterResult messageRouterResult)
        {
            Party conversationOwnerParty = messageRouterResult.ConversationOwnerParty;

            string message = $"{(string.IsNullOrEmpty(messageRouterResult.ErrorMessage) ? "Failed to forward the message" : messageRouterResult.ErrorMessage)}";
            string eventText = (conversationOwnerParty?.AllowBackchannel ?? false) ?
                BackChannelCommands.GetMessageRouter(MessageRouterResultType.FailedToForwardMessage)
                : string.Empty;

            await MessageRouterManager.SendMessageToPartyByBotAsync(conversationOwnerParty, message, eventText);
        }

        /// <summary>
        /// Notifies the user that there are no aggregation channels setup 
        /// </summary>
        /// <param name="messageRouterResult">The result to handle.</param>
        private async Task<string> HandleNoAggregationChannelResultAsync(MessageRouterResult messageRouterResult)
        {
            string message = string.Empty;

            if (messageRouterResult.Activity != null)
            {
                string botName = RoutingDataManager.ResolveBotNameInConversation(
                    MessagingUtils.CreateSenderParty(messageRouterResult.Activity));

                message = $"{(string.IsNullOrEmpty(messageRouterResult.ErrorMessage) ? "" : $"{messageRouterResult.ErrorMessage}: ")}The message router manager is not initialized; type \"";
                message += string.IsNullOrEmpty(botName) ? $"{Commands.CommandKeyword} " : $"@{botName} ";
                message += $"{Commands.CommandAddAggregationChannel}\" to setup the aggregation channel";

                await MessagingUtils.ReplyToActivityAsync(messageRouterResult.Activity, message);
            }
            else
            {
                Debug.WriteLine("The activity of the result is null");
            }

            return message;
        }

        /// <summary>
        /// Notifies the conversation client (customer) that no agents are available 
        /// (i.e. no agents are currently watching for requests
        /// </summary>
        /// <param name="messageRouterResult">The result to handle.</param>
        private async Task HandleNoAgentsAvailableResultAsync(MessageRouterResult messageRouterResult)
        {
            Party conversationOwnerParty = messageRouterResult.ConversationOwnerParty;

            string message = AeccStrings.AddNoAgentsAvailable_Text;
            string eventText = (conversationOwnerParty?.AllowBackchannel ?? false) ?
                BackChannelCommands.GetMessageRouter(MessageRouterResultType.NoAgentsAvailable)
                : string.Empty;

            await MessageRouterManager.SendMessageToPartyByBotAsync(conversationOwnerParty, message, eventText);
        }

        /// <summary>
        /// Notifies both the conversation owner (agent) and the conversation client (customer)
        /// about the change in engagement (initiated/started/ended).
        /// </summary>
        /// <param name="messageRouterResult">The result to handle.</param>
        protected virtual async Task HandleConnectionChangedResultAsync(MessageRouterResult messageRouterResult)
        {
            Party conversationOwnerParty = messageRouterResult.ConversationOwnerParty;
            Party conversationClientParty = messageRouterResult.ConversationClientParty;

            string conversationOwnerName = conversationOwnerParty?.ChannelAccount.Name;
            string conversationClientName = conversationClientParty?.ChannelAccount.Name;

            string messageToConversationOwner = string.Empty;
            string messageToConversationClient = string.Empty;

            string eventText = BackChannelCommands.GetMessageRouter(messageRouterResult.Type);
            string eventToConversationOwner = (conversationOwnerParty?.AllowBackchannel ?? false) ?
                eventText : string.Empty;

            string eventToConversationClient = (conversationClientParty?.AllowBackchannel ?? false) ?
                eventText : string.Empty;

            if (messageRouterResult.Type == MessageRouterResultType.ConnectionRequested)
            {
                Party aggregationParty = null;
                int selectedIndex = 0;
                if (int.TryParse(messageRouterResult.SelectedOption, out selectedIndex))
                {
                    var aggregationParties = RoutingDataManager.GetAggregationParties();
                    if (aggregationParties.Count >= selectedIndex)
                    {
                        aggregationParty = aggregationParties[selectedIndex - 1];
                    }
                }
                else
                {
                    aggregationParty = Party.FromJsonString(messageRouterResult.SelectedOption);
                }

                if (aggregationParty != null)
                {
                    IMessageActivity messageActivity = (!aggregationParty.AllowBackchannel) ?
                        CreateRequestCard(conversationClientParty, aggregationParty) :
                        CreateRequestBackchannel(conversationClientParty, aggregationParty);

                    await MessageRouterManager.SendMessageToPartyByBotAsync(aggregationParty, messageActivity);

                    eventToConversationClient = string.Empty;
                    messageToConversationClient = AeccStrings.EngagementInitiated_ClientText;
                }
                else
                    messageToConversationClient = AeccStrings.EngagementInitiated_AggregationNotFoundClientText;
            }
            else if (messageRouterResult.Type == MessageRouterResultType.ConnectionAlreadyRequested)
            {
                string commandKeyword = AeccStrings.GetCommandKeyword(conversationClientParty);

                string rejectCommand = $"{commandKeyword} {Commands.CommandRejectRequest}";
                eventToConversationClient = string.Empty;
                messageToConversationClient = (conversationClientParty.AllowBackchannel) ?
                    AeccStrings.EngagementInitiated_AlreadyClientEventText :
                    string.Format(AeccStrings.EngagementInitiated_AlreadyClientText, rejectCommand);
            }
            else if (messageRouterResult.Type == MessageRouterResultType.ConnectionRejected)
            {
                if (conversationClientParty != conversationOwnerParty)
                {
                    messageToConversationOwner = string.Format(AeccStrings.RejectRequest_OwnerText, conversationClientName);
                    messageToConversationClient = AeccStrings.RejectRequest_ClientText;
                }
                else
                {
                    messageToConversationClient = AeccStrings.RejectRequest_AlreadyRejectClientText;
                }
            }
            else if (messageRouterResult.Type == MessageRouterResultType.Connected)
            {
                string commandKeyword = AeccStrings.GetCommandKeyword(conversationClientParty, conversationOwnerParty);
                string endEngagementCommand = $"{commandKeyword} {Commands.CommandEndEngagement}";

                eventToConversationClient = string.Empty;
                eventToConversationOwner = string.Empty;
                messageToConversationOwner = (conversationOwnerParty.AllowBackchannel) ?
                    string.Format(AeccStrings.EngagementAdded_OwnerEventText, conversationClientName) :
                    string.Format(AeccStrings.EngagementAdded_OwnerText, conversationClientName, endEngagementCommand);
                messageToConversationClient = string.Format(AeccStrings.EngagementAdded_ClientText, conversationOwnerName);
            }
            else if (messageRouterResult.Type == MessageRouterResultType.Disconnected)
            {
                string commandKeyword = AeccStrings.GetCommandKeyword(conversationClientParty, conversationOwnerParty);
                string deleteAggregationChannelCommand = $"{commandKeyword} {Commands.CommandDeleteAggregationChannel}";

                messageToConversationOwner = (conversationOwnerParty.AllowBackchannel) ?
                    string.Format(AeccStrings.EngagementRemoved_OwnerEventText, conversationClientName) :
                    string.Format(AeccStrings.EngagementRemoved_OwnerText, conversationClientName, deleteAggregationChannelCommand);
                messageToConversationClient = string.Format(AeccStrings.EngagementRemoved_ClientText, conversationOwnerName);
            }

            if (!string.IsNullOrEmpty(messageToConversationOwner))
            {
                await MessageRouterManager.SendMessageToPartyByBotAsync(conversationOwnerParty, messageToConversationOwner, eventToConversationOwner);
            }

            if (!string.IsNullOrEmpty(messageToConversationClient))
            {
                await MessageRouterManager.SendMessageToPartyByBotAsync(conversationClientParty, messageToConversationClient, eventToConversationClient);
            }
        }

        /// <summary>
        /// Creates a new IMessageActivity containing the buttons (and additional information)
        /// to either accept or reject a pending engagement request.
        /// 
        /// Note that the created IMessageActivity will not contain valid From property value!
        /// However, if you use MessageRouterManager.SendMessageToPartyByBotAsync(),
        /// it will set the from field.
        /// </summary>
        /// <param name="pendingRequest">The party with a pending request (i.e. customer/client).</param>
        /// <param name="aggregationParty">The aggregation party to notify about the request.</param>
        /// <returns>A newly created IMessageActivity instance.</returns>
        protected virtual IMessageActivity CreateRequestCard(Party pendingRequest, Party aggregationParty)
        {
            if (pendingRequest == null || pendingRequest.ChannelAccount == null || aggregationParty == null)
            {
                throw new ArgumentNullException("The given arguments do not have the necessary details");
            }

            IMessageActivity messageActivity = Activity.CreateMessageActivity();
            messageActivity.Conversation = aggregationParty.ConversationAccount;
            messageActivity.Recipient = pendingRequest.ChannelAccount;

            string requesterId = pendingRequest.ChannelAccount.Id;
            string requesterName = pendingRequest.ChannelAccount.Name;
            string commandKeyword = AeccStrings.GetCommandKeyword(pendingRequest, aggregationParty);
            string acceptCommand = $"{commandKeyword} {Commands.CommandAcceptRequest} {requesterId}";
            string rejectCommand = $"{commandKeyword} {Commands.CommandRejectRequest} {requesterId}";

            ThumbnailCard thumbnailCard = new ThumbnailCard()
            {
                Title = AeccStrings.RequestCard_Title,
                Subtitle = string.Format(AeccStrings.RequestCard_Subtitle, requesterName),
                Text = AeccStrings.RequestCard_Text,
                Buttons = new List<CardAction>()
                {
                    new CardAction()
                    {
                        Title = AeccStrings.RequestCard_AcceptTitle,
                        Type = ActionTypes.PostBack,
                        Value = acceptCommand
                    },
                    new CardAction()
                    {
                        Title = AeccStrings.RequestCard_RejectTitle,
                        Type = ActionTypes.PostBack,
                        Value = rejectCommand
                    }
                }
            };

            messageActivity.Attachments = new List<Attachment>() { thumbnailCard.ToAttachment() };
            return messageActivity;
        }

        protected IMessageActivity CreateRequestBackchannel(Party pendingRequest, Party aggregationParty)
        {
            if (pendingRequest == null || pendingRequest.ChannelAccount == null || aggregationParty == null)
            {
                throw new ArgumentNullException("The given arguments do not have the necessary details");
            }

            IMessageActivity messageActivity = Activity.CreateMessageActivity();
            messageActivity.Conversation = aggregationParty.ConversationAccount;
            messageActivity.Recipient = pendingRequest.ChannelAccount;

            messageActivity.Type = ActivityTypes.Event;
            messageActivity.Text = BackChannelCommands.GetMessageRouter(MessageRouterResultType.ConnectionRequested);
            messageActivity.ChannelData = pendingRequest.ToJsonString();
            return messageActivity;
        }

        protected virtual IMessageActivity CreateAggregationsCard(Party pendingRequest, Party aggregationParty)
        {
            if (pendingRequest == null || pendingRequest.ChannelAccount == null || aggregationParty == null)
            {
                throw new ArgumentNullException("The given arguments do not have the necessary details");
            }

            IMessageActivity messageActivity = Activity.CreateMessageActivity();
            messageActivity.Conversation = aggregationParty.ConversationAccount;
            messageActivity.Recipient = pendingRequest.ChannelAccount;

            string requesterId = pendingRequest.ChannelAccount.Id;
            string requesterName = pendingRequest.ChannelAccount.Name;

            string commandKeyword = AeccStrings.GetCommandKeyword(pendingRequest, aggregationParty);
            string acceptCommand = $"{commandKeyword} {Commands.CommandAcceptRequest} {requesterId}";
            string rejectCommand = $"{commandKeyword} {Commands.CommandRejectRequest} {requesterId}";

            ThumbnailCard thumbnailCard = new ThumbnailCard()
            {
                Title = $"Hola {pendingRequest.ConversationAccount.Name}. Indícanos con quién quieres hablar.",
                Subtitle = $"Voluntarios disponibles",
                Text = $"Utiliza los botones para seleccionar un voluntario.",
                Buttons = new List<CardAction>()
                {
                    new CardAction()
                    {
                        Title = "Accept",
                        Type = ActionTypes.PostBack,
                        Value = acceptCommand
                    },
                    new CardAction()
                    {
                        Title = "Reject",
                        Type = ActionTypes.PostBack,
                        Value = rejectCommand
                    }
                }
            };

            messageActivity.Attachments = new List<Attachment>() { thumbnailCard.ToAttachment() };
            return messageActivity;
        }

    }
}