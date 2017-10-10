using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Bot.Connector;
using AeccBot.MessageRouting.DataStore;
using AeccBot.Models;
using AeccBot.Utils;
using AeccBot.MessageRouting;
using System.Collections.Generic;

namespace AeccBot.CommandHandling
{
    public class BackChannelMessageHandler : IBotCommandHandler
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
        /// Handler for the results of the more complex operations of this class.
        /// </summary>
        private IMessageRouterResultHandler MessageRouterResultHandler
        {
            get { return WebApiConfig.MessageRouterResultHandler; }
        }

        public async Task<bool> HandleCommandAsync(Activity activity)
        {
            bool wasHandled = false;
            Activity replyActivity = null;

            if (string.IsNullOrEmpty(activity.Text) || !activity.Text.StartsWith($"{BackChannelCommands.BackChannelKeyword} "))
                return wasHandled;

            var messagePart = activity.Text.Remove(0, BackChannelCommands.BackChannelKeyword.Length + 1).Split(' ');
            if (!messagePart.Any())
                return wasHandled;

            switch (messagePart.First())
            {
                case BackChannelCommands.CommandAddAggregation:
                    replyActivity = await HandleCommandAddAggregationChannelAsync(activity);
                    wasHandled = true;
                    break;
                case BackChannelCommands.CommandDeleteAggregation:
                    replyActivity = HandleCommandDeleteAggregationChannel(activity);
                    wasHandled = true;
                    break;
                case BackChannelCommands.CommandCheckAggregation:
                    replyActivity = HandleCommandCheckAggregationChannel(activity);
                    wasHandled = true;
                    break;
                case BackChannelCommands.CommandAcceptRequest:
                    replyActivity = await HandleCommandAcceptAndRejectRequestAsync(activity, true);
                    wasHandled = true;
                    break;
                case BackChannelCommands.CommandRejectRequest:
                    replyActivity = await HandleCommandAcceptAndRejectRequestAsync(activity, false);
                    wasHandled = true;
                    break;
                case BackChannelCommands.CommandListAggregations:
                    replyActivity = HandleCommandListAggregations(activity);
                    wasHandled = true;
                    break;
                case BackChannelCommands.CommandInitiateEngagement:
                    replyActivity = await HandleCommandInitiateEngagementAsync(activity);
                    wasHandled = true;
                    break;
                case BackChannelCommands.CommandEndEngagement:
                    replyActivity = await HandleCommandEndEngagementAsync(activity);
                    wasHandled = true;
                    break;
                case BackChannelCommands.CommandSyncEngagement:
                    await HandleCommandSyncEngagementAsync(activity);
                    wasHandled = true;
                    break;
                case BackChannelCommands.CommandEngagementCounterpart:
                    replyActivity = HandleCommandEngagementCounterpart(activity);
                    wasHandled = true;
                    break;
                default:
                    break;
            }

            if (replyActivity != null)
            {
                ConnectorClient connector = new ConnectorClient(new Uri(activity.ServiceUrl));
                await connector.Conversations.ReplyToActivityAsync(replyActivity);
            }

            return wasHandled;
        }

        private Activity HandleCommandEngagementCounterpart(Activity activity)
        {
            Activity replyActivity = activity.CreateReply(activity.Text);
            replyActivity.Type = ActivityTypes.Event;

            Party senderParty = MessagingUtils.CreateSenderParty(activity);

            replyActivity.ChannelData = RoutingDataManager.GetConnectedCounterpart(senderParty);

            return replyActivity;
        }

        private async Task HandleCommandSyncEngagementAsync(Activity activity)
        {
            Party senderParty = MessagingUtils.CreateSenderParty(activity);
            Party counterpartParty = RoutingDataManager.GetConnectedCounterpart(senderParty);
            if (counterpartParty != null && counterpartParty.AllowBackchannel)
            {
                IMessageActivity eventActivity = Activity.CreateMessageActivity();
                eventActivity.Text = activity.Text;
                eventActivity.ChannelData = activity.ChannelData;
                eventActivity.Conversation = counterpartParty.ConversationAccount;
                eventActivity.Recipient = counterpartParty.ChannelAccount;
                eventActivity.Type = ActivityTypes.Event;

                await MessageRouterManager.SendMessageToPartyByBotAsync(counterpartParty, eventActivity);
            }
        }

        private async Task<Activity> HandleCommandAddAggregationChannelAsync(Activity activity)
        {
            Activity replyActivity = activity.CreateReply(activity.Text);
            replyActivity.Type = ActivityTypes.Event;

            // Check if the Aggregation Party already exists
            Party aggregationParty = MessagingUtils.CreateSenderParty(activity);

            if (RoutingDataManager.IsConnected(aggregationParty, ConnectionProfile.Owner))
            {
                Party otherParty = RoutingDataManager.GetConnectedCounterpart(aggregationParty);

                if (otherParty != null)
                {
                    replyActivity.ChannelData = string.Format(AeccStrings.AcceptAndRejectRequest_InOtherConversation, otherParty.ChannelAccount.Name);
                }
            }
            // Establish the sender's channel/conversation as an aggreated one
            else if (RoutingDataManager.AddAggregationParty(aggregationParty))
            {
                foreach (var party in RoutingDataManager.GetUserParties())
                {
                    if (RoutingDataManager.IsPendingRequest(party)
                        || RoutingDataManager.IsInAggregation(party)
                        || party.AllowBackchannel)
                    {
                        continue;
                    }
                    await MessageRouterManager.SendMessageToPartyByBotAsync(party, CreateAggregationNotification(party, aggregationParty));
                }
            }
      
            if (replyActivity.ChannelData == null)
            {
                replyActivity.ChannelData = true;
            }
            return replyActivity;
        }

        private Activity HandleCommandDeleteAggregationChannel(Activity activity)
        {
            Activity replyActivity = activity.CreateReply(activity.Text);
            replyActivity.Type = ActivityTypes.Event;

            // Check if the Aggregation Party already exists
            Party aggregationParty = MessagingUtils.CreateSenderParty(activity);

            if (RoutingDataManager.IsConnected(aggregationParty, ConnectionProfile.Owner))
            {
                replyActivity.ChannelData = AeccStrings.DeleteAggregationChannel_InEngagementEventText;
            }
            // Establish the sender's channel/conversation as an aggreated one
            else if (RoutingDataManager.IsInAggregation(aggregationParty))
            {
                RoutingDataManager.RemoveAggregationParty(aggregationParty);
            }

            if (replyActivity.ChannelData == null)
            {
                replyActivity.ChannelData = true;
            }
            return replyActivity;
        }

        private Activity HandleCommandCheckAggregationChannel(Activity activity)
        {
            Activity replyActivity = activity.CreateReply(activity.Text);
            replyActivity.Type = ActivityTypes.Event;

            Party aggregationParty = MessagingUtils.CreateSenderParty(activity);

            if (!RoutingDataManager.IsInAggregation(aggregationParty) &&
                !RoutingDataManager.IsConnected(aggregationParty, ConnectionProfile.Owner))
            {
                RoutingDataManager.AddAggregationParty(aggregationParty);
            }

            replyActivity.ChannelData = RoutingDataManager.UpdateCheckAggregationParty(aggregationParty);

            return replyActivity;
        }

        private async Task<Activity> HandleCommandAcceptAndRejectRequestAsync(Activity activity, bool doAccept)
        {
            Activity replyActivity = activity.CreateReply(activity.Text);
            replyActivity.Type = ActivityTypes.Event;

            // Accept/reject conversation request
            Party senderParty = MessagingUtils.CreateSenderParty(activity);

            if (RoutingDataManager.IsAssociatedWithAggregation(senderParty)
                || RoutingDataManager.IsConnected(senderParty, ConnectionProfile.Owner))
            {
                replyActivity.ChannelData = await HandleCommandAcceptAndRejectRequestAsync(activity, doAccept, senderParty);
            }
            else if (RoutingDataManager.IsPendingRequest(senderParty))
            {
                var messageRouterResult = MessageRouterManager.RejectPendingRequest(senderParty, senderParty);
                await MessageRouterResultHandler.HandleResultAsync(messageRouterResult);
            }

            if (replyActivity.ChannelData == null)
            {
                replyActivity.ChannelData = true;
            }
            return replyActivity;
        }

        private async Task<string> HandleCommandAcceptAndRejectRequestAsync(Activity activity, bool doAccept, Party senderParty)
        {
            string result = null;

            // The party is associated with the aggregation and has the right to accept/reject
            Party senderInConversation = MessagingUtils.CreateSenderParty(activity);

            if (senderInConversation == null || !RoutingDataManager.IsConnected(senderInConversation, ConnectionProfile.Owner))
            {
                var pendingRequest = RoutingDataManager.GetPendingRequests();
                if (pendingRequest.Any() && activity.ChannelData != null)
                {
                    Party partyBulk = Party.FromJsonString(activity.ChannelData.ToString());
                    Party partyToAcceptOrReject = MessagingUtils.GetReferenceParty(partyBulk);

                    // The name of the user to accept
                    if (partyToAcceptOrReject != null && pendingRequest.Contains(partyToAcceptOrReject))
                    {
                        MessageRouterResult messageRouterResult = null;

                        if (doAccept)
                        {
                            messageRouterResult = await MessageRouterManager.ConnectAsync(senderParty, partyToAcceptOrReject, false);
                        }
                        else
                        {
                            messageRouterResult = MessageRouterManager.RejectPendingRequest(partyToAcceptOrReject, senderParty);
                        }

                        await MessageRouterResultHandler.HandleResultAsync(messageRouterResult);
                    }
                    else
                    {
                        result = string.Format(AeccStrings.AcceptAndRejectRequest_PartyNotFound, partyToAcceptOrReject?.ChannelAccount.Name);
                    }
                }
                else
                {
                    result = AeccStrings.AcceptAndRejectRequest_NoRequest;
                }
            }
            else
            {
                Party otherParty = RoutingDataManager.GetConnectedCounterpart(senderInConversation);
                if (otherParty == null)
                {
                    result = AeccStrings.AcceptAndRejectRequest_Error;
                }
            }

            return result;
        }

        protected virtual IMessageActivity CreateAggregationNotification(Party userParty, Party aggregationParty)
        {
            IMessageActivity messageActivity = Activity.CreateMessageActivity();
            messageActivity.Conversation = userParty.ConversationAccount;
            messageActivity.Recipient = userParty.ChannelAccount;

            messageActivity.Text = string.Format(AeccStrings.AddAggregationChannel_NotifyText, aggregationParty.ChannelAccount.Name);

            return messageActivity;
        }

        private async Task<Activity> HandleCommandEndEngagementAsync(Activity activity)
        {
            Activity replyActivity = activity.CreateReply(activity.Text);
            replyActivity.Type = ActivityTypes.Event;

            // End the 1:1 conversation
            Party senderParty = MessagingUtils.CreateSenderParty(activity);

            IList<MessageRouterResult> messageRouterResults = MessageRouterManager.Disconnect(senderParty);

            foreach (MessageRouterResult messageRouterResult in messageRouterResults)
            {
                await MessageRouterResultHandler.HandleResultAsync(messageRouterResult);
            }

            replyActivity.ChannelData = MessageRouterResult.IsOk(messageRouterResults);
            return replyActivity;
        }

        private Activity HandleCommandListAggregations(Activity activity)
        {
            Activity replyActivity = activity.CreateReply(activity.Text);
            replyActivity.Type = ActivityTypes.Event;
            replyActivity.ChannelData = RoutingDataManager.GetAggregationParties();

            return replyActivity;
        }

        private async Task<Activity> HandleCommandInitiateEngagementAsync(Activity activity)
        {
            Activity replyActivity = activity.CreateReply(activity.Text);
            replyActivity.Type = ActivityTypes.Event;
         
            MessageRouterResult messageRouterResult = null;

            if (activity.ChannelData != null)
            {
                string selectedPartyId = activity.ChannelData.ToString();
                messageRouterResult = MessageRouterManager.RequestConnection(activity, selectedPartyId);
            }

            // Handle the result, if required
            await MessageRouterResultHandler.HandleResultAsync(messageRouterResult);

            bool result = (messageRouterResult?.Type == MessageRouterResultType.ConnectionRequested);

            replyActivity.ChannelData = result;
            return replyActivity;
        }
    }
}