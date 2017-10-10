using AeccBot.MessageRouting;
using AeccBot.MessageRouting.DataStore;
using AeccBot.Models;
using AeccBot.Utils;
using Microsoft.Bot.Connector;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

/// <summary>
/// Code base in
/// </summary>
namespace AeccBot.CommandHandling
{
    /// <summary>
    /// The default handler for bot commands related to message routing.
    /// </summary>
    public class CommandMessageHandler : IBotCommandHandler
    {
        private MessageRouterManager MessageRouterManager
        {
            get{ return  WebApiConfig.MessageRouterManager; }
        }
        
        private IRoutingDataManager RoutingDataManager
        {
            get { return WebApiConfig.RoutingDataManager; }
        }
        
        private IMessageRouterResultHandler MessageRouterResultHandler
        {
            get { return WebApiConfig.MessageRouterResultHandler; }
        }

        /// <summary>
        /// All messages where the bot was mentioned ("@<bot name>") are checked for possible commands.
        /// See IBotCommandHandler.cs for more information.
        /// </summary>
        public async Task<bool> HandleCommandAsync(Activity activity)
        {
            bool wasHandled = false;
            Activity replyActivity = null;

            if (!WasBotAddressedDirectly(activity) &&
               (string.IsNullOrEmpty(activity.Text) || !activity.Text.StartsWith($"{Commands.CommandKeyword} ", StringComparison.InvariantCultureIgnoreCase)))
                return wasHandled;

            string message = MessagingUtils.StripMentionsFromMessage(activity);

            if (message.StartsWith($"{Commands.CommandKeyword} ", StringComparison.InvariantCultureIgnoreCase))
            {
                message = message.Remove(0, Commands.CommandKeyword.Length + 1);
            }

            string messageInLowerCase = message?.ToLower();

            if (messageInLowerCase.StartsWith(Commands.CommandAddAggregationChannel))
            {
                replyActivity = await HandleCommandAddAggregationChannelAsync(activity);
                wasHandled = true;
            }
            else if (messageInLowerCase.StartsWith(Commands.CommandDeleteAggregationChannel))
            {
                replyActivity = HandleCommandDeleteAggregationChannel(activity);
                wasHandled = true;
            }
            else if (messageInLowerCase.StartsWith(Commands.CommandAcceptRequest)
                || messageInLowerCase.StartsWith(Commands.CommandRejectRequest))
            {
                // Accept/reject conversation request
                bool doAccept = messageInLowerCase.StartsWith(Commands.CommandAcceptRequest);
                Party senderParty = MessagingUtils.CreateSenderParty(activity);

                if (RoutingDataManager.IsAssociatedWithAggregation(senderParty)
                    || RoutingDataManager.IsConnected(senderParty, ConnectionProfile.Owner))
                {
                    replyActivity = await HandleCommandAcceptAndRejectRequestAsync(activity, message, doAccept, senderParty);
                    wasHandled = true;
                }
                else if (RoutingDataManager.IsPendingRequest(senderParty))
                {
                    var messageRouterResult = MessageRouterManager.RejectPendingRequest(senderParty, senderParty);
                    await MessageRouterResultHandler.HandleResultAsync(messageRouterResult);
                }
                else
                {
                    replyActivity = HandleNoAggregation(activity, senderParty);
                    wasHandled = true;
                }
            }
            else if (messageInLowerCase.StartsWith(Commands.CommandEndEngagement))
            {
                replyActivity = await HandleCommandEndEngagementAsync(activity);
                wasHandled = true;
            }
            /*
            * NOTE: Either remove these commands or make them unaccessible should you use this
            * code in production!
            */
            #region Commands for debugging
            else if (messageInLowerCase.StartsWith(Commands.CommandDeleteAllRoutingData))
            {
                ConnectorClient connector = new ConnectorClient(new Uri(activity.ServiceUrl));
                await connector.Conversations.ReplyToActivityAsync(activity.CreateReply("Deleting all data..."));

                RoutingDataManager.DeleteAll();

                wasHandled = true;
            }
#if DEBUG
            else if (messageInLowerCase.StartsWith(Commands.CommandListAllParties))
            {
                string replyMessage = string.Empty;
                string parties = string.Empty;

                foreach (Party userParty in RoutingDataManager.GetUserParties())
                {
                    parties += userParty.ToString() + "\n";
                }

                if (string.IsNullOrEmpty(parties))
                {
                    replyMessage = "No user parties;\n";
                }
                else
                {
                    replyMessage = "Users:\n" + parties;
                }

                parties = string.Empty;

                foreach (Party botParty in RoutingDataManager.GetBotParties())
                {
                    parties += botParty.ToString() + "\n";
                }

                if (string.IsNullOrEmpty(parties))
                {
                    replyMessage += "No bot parties";
                }
                else
                {
                    replyMessage += "Bot:\n" + parties;
                }

                replyActivity = activity.CreateReply(replyMessage);
                wasHandled = true;
            }
            else if (messageInLowerCase.StartsWith(Commands.CommandListPendingRequests))
            {
                string parties = string.Empty;

                foreach (Party party in RoutingDataManager.GetPendingRequests())
                {
                    parties += party.ToString() + "\n";
                }

                if (parties.Length == 0)
                {
                    parties = "No pending requests";
                }

                replyActivity = activity.CreateReply(parties);
                wasHandled = true;
            }
            else if (messageInLowerCase.StartsWith(Commands.CommandListEngagements))
            {
                string parties = RoutingDataManager.ConnectionsToString();

                if (string.IsNullOrEmpty(parties))
                {
                    replyActivity = activity.CreateReply("No conversations");
                }
                else
                {
                    replyActivity = activity.CreateReply(parties);
                }

                wasHandled = true;
            }

            else if (messageInLowerCase.StartsWith(Commands.CommandListLastMessageRouterResults))
            {
                LocalRoutingDataManager routingDataManager = RoutingDataManager as LocalRoutingDataManager;

                if (routingDataManager != null)
                {
                    string resultsAsString = routingDataManager.GetLastMessageRouterResults();
                    replyActivity = activity.CreateReply($"{(string.IsNullOrEmpty(resultsAsString) ? "No results" : resultsAsString)}");
                    wasHandled = true;
                }
            }
#endif
            #endregion Commands for debugging

            else
            {
                replyActivity = activity.CreateReply(string.Format(AeccStrings.HandleCommand_NotFound, messageInLowerCase));
                wasHandled = true;
            }

            if (replyActivity != null)
            {
                ConnectorClient connector = new ConnectorClient(new Uri(activity.ServiceUrl));
                await connector.Conversations.ReplyToActivityAsync(replyActivity);
            }

            return wasHandled;
        }

        private Activity HandleNoAggregation(Activity activity, Party senderParty)
        {
            string commandKeyword = AeccStrings.GetCommandKeyword(senderParty);
            string aggregationCommand = $"{commandKeyword} {Commands.CommandAddAggregationChannel}";

            Activity replyActivity = activity.CreateReply(string.Format(AeccStrings.AcceptAndRejectRequest_NoAggregation, aggregationCommand));
            return replyActivity;
        }

        private async Task<Activity> HandleCommandAddAggregationChannelAsync(Activity activity)
        {
            Activity replyActivity = null;
            // Check if the Aggregation Party already exists
            Party aggregationParty = MessagingUtils.CreateSenderParty(activity);

            if (RoutingDataManager.IsConnected(aggregationParty, ConnectionProfile.Owner))
            {
                Party otherParty = RoutingDataManager.GetConnectedCounterpart(aggregationParty);

                if (otherParty != null)
                {
                    replyActivity = activity.CreateReply(string.Format(AeccStrings.AcceptAndRejectRequest_InOtherConversation, otherParty.ChannelAccount.Name));
                }
                else
                {
                    replyActivity = activity.CreateReply(AeccStrings.AcceptAndRejectRequest_Error);
                }
            }
            // Establish the sender's channel/conversation as an aggreated one
            else if (RoutingDataManager.AddAggregationParty(aggregationParty))
            {
                replyActivity = activity.CreateReply(AeccStrings.AddAggregationChannel_Text);

                foreach (var party in RoutingDataManager.GetUserParties())
                {
                    if (RoutingDataManager.IsPendingRequest(party)
                        || RoutingDataManager.IsInAggregation(party)
                        || aggregationParty.AllowBackchannel)
                    {
                        continue;
                    }
                    await MessageRouterManager.SendMessageToPartyByBotAsync(party, CreateAggregationNotification(party, aggregationParty));
                }
            }
            else
            {
                // Aggregation already exists
                replyActivity = activity.CreateReply(AeccStrings.AddAggregationChannel_AlreadyExistsText);
            }

            return replyActivity;
        }

        private Activity HandleCommandDeleteAggregationChannel(Activity activity)
        {
            Activity replyActivity;

            // Check if the Aggregation Party already exists
            Party aggregationParty = MessagingUtils.CreateSenderParty(activity);

            string commandKeyword = AeccStrings.GetCommandKeyword(aggregationParty);
            string aggregationCommand = $"{commandKeyword} {Commands.CommandAddAggregationChannel}";

            if (RoutingDataManager.IsConnected(aggregationParty, ConnectionProfile.Owner))
            {
                string endEngagementCommand = $"{commandKeyword} {Commands.CommandEndEngagement}";
                replyActivity = activity.CreateReply(string.Format(AeccStrings.DeleteAggregationChannel_InEngagementText, endEngagementCommand));
            }
            // Establish the sender's channel/conversation as an aggreated one
            else if (RoutingDataManager.IsInAggregation(aggregationParty))
            {
                RoutingDataManager.RemoveAggregationParty(aggregationParty);

                replyActivity = activity.CreateReply(string.Format(AeccStrings.DeleteAggregationChannel_Text, aggregationCommand));
            }
            else
            {
                // Aggregation not exists
                replyActivity = activity.CreateReply(string.Format(AeccStrings.DeleteAggregationChannel_NotExistText, aggregationCommand));
            }

            return replyActivity;
        }

        private async Task<Activity> HandleCommandEndEngagementAsync(Activity activity)
        {
            Activity replyActivity = null;

            // End the 1:1 conversation
            Party senderParty = MessagingUtils.CreateSenderParty(activity);

            IList<MessageRouterResult> messageRouterResults = MessageRouterManager.Disconnect(senderParty);

            foreach (MessageRouterResult messageRouterResult in messageRouterResults)
            {
                await MessageRouterResultHandler.HandleResultAsync(messageRouterResult);
            }

            if (!MessageRouterResult.IsOk(messageRouterResults))
            {
                replyActivity = activity.CreateReply(AeccStrings.EndEngagement_ErrorText);
            }

            return replyActivity;
        }

        private async Task<Activity> HandleCommandAcceptAndRejectRequestAsync(Activity activity, string message, bool doAccept, Party senderParty)
        {
            Activity replyActivity = null;
            // The party is associated with the aggregation and has the right to accept/reject
            Party senderInConversation =
                RoutingDataManager.FindEngagedPartyByChannel(senderParty.ChannelId, senderParty.ChannelAccount);

            if (senderInConversation == null || !RoutingDataManager.IsConnected(senderInConversation, ConnectionProfile.Owner))
            {
                var pendingRequest = RoutingDataManager.GetPendingRequests();
                if (pendingRequest.Any())
                {
                    // The name of the user to accept should be the second word
                    string[] splitMessage = message.Split(' ');

                    if (splitMessage.Count() > 1 && !string.IsNullOrEmpty(splitMessage[1]))
                    {
                        Party partyToAcceptOrReject = null;

                        try
                        {
                            partyToAcceptOrReject = pendingRequest.Single(
                                  party => (party.ChannelAccount != null
                                      && !string.IsNullOrEmpty(party.ChannelAccount.Id)
                                      && party.ChannelAccount.Id.Equals(splitMessage[1])));
                        }
                        catch (InvalidOperationException e)
                        {
                            Debug.WriteLine(e);
                        }

                        if (partyToAcceptOrReject != null)
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
                            replyActivity = activity.CreateReply(string.Format(AeccStrings.AcceptAndRejectRequest_PartyNotFound, splitMessage[1]));
                        }
                    }
                    else
                    {
                        replyActivity = activity.CreateReply(AeccStrings.AcceptAndRejectRequest_UserNotFound);
                    }
                }
                else
                {
                    replyActivity = activity.CreateReply(AeccStrings.AcceptAndRejectRequest_NoRequest);
                }
            }
            else
            {
                Party otherParty = RoutingDataManager.GetConnectedCounterpart(senderInConversation);

                if (otherParty != null)
                {
                    replyActivity = activity.CreateReply(string.Format(AeccStrings.AcceptAndRejectRequest_InOtherConversation, otherParty.ChannelAccount.Name));
                }
                else
                {
                    replyActivity = activity.CreateReply(AeccStrings.AcceptAndRejectRequest_Error);
                }
            }

            return replyActivity;
        }

        /// <summary>
        /// Checks the given activity and determines whether the message was addressed directly to
        /// the bot or not.
        /// 
        /// Note: Only mentions are inspected at the moment.
        /// </summary>
        /// <param name="messageActivity">The message activity.</param>
        /// <returns>True, if the message was address directly to the bot. False otherwise.</returns>
        protected bool WasBotAddressedDirectly(IMessageActivity messageActivity)
        {
            bool botWasMentioned = false;
            Mention[] mentions = messageActivity.GetMentions();

            foreach (Mention mention in mentions)
            {
                foreach (Party botParty in RoutingDataManager.GetBotParties())
                {
                    if (mention.Mentioned.Id.Equals(botParty.ChannelAccount.Id))
                    {
                        botWasMentioned = true;
                        break;
                    }
                }
            }

            return botWasMentioned;
        }

        protected virtual IMessageActivity CreateAggregationNotification(Party userParty, Party aggregationParty)
        {
            IMessageActivity messageActivity = Activity.CreateMessageActivity();
            messageActivity.Conversation = userParty.ConversationAccount;
            messageActivity.Recipient = userParty.ChannelAccount;

            messageActivity.Text = string.Format(AeccStrings.AddAggregationChannel_NotifyText, aggregationParty.ChannelAccount.Name);

            return messageActivity;
        }

    }
}