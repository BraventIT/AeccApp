﻿using AeccBot.CommandHandling;
using AeccBot.MessageRouting.DataStore;
using AeccBot.Models;
using AeccBot.Utils;
using Microsoft.Bot.Connector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AeccBot.MessageRouting
{
    // <summary>
    /// Provides the main interface for message routing.
    /// </summary>
    public class MessageRouterManager
    {
        /// <summary>
        /// The routing data and all the parties the bot has seen including the instances of itself.
        /// </summary>
        private IRoutingDataManager RoutingDataManager
        {
            get { return WebApiConfig.RoutingDataManager; }
        }

        /// <summary>
        /// Tries to send the given message activity to the given party using this bot on the same
        /// channel as the party who the message is sent to.
        /// </summary>
        /// <param name="partyToMessage">The party to send the message to.</param>
        /// <param name="messageActivity">The message activity to send (message content).</param>
        /// <returns>The ResourceResponse instance or null in case of an error.</returns>
        public async Task<ResourceResponse> SendMessageToPartyByBotAsync(
            Party partyToMessage, IMessageActivity messageActivity)
        {
            Party botParty = null;

            if (partyToMessage != null)
            {
                // We need the channel account of the bot in the SAME CHANNEL as the RECIPIENT.
                // The identity of the bot in the channel of the sender is most likely a different one and
                // thus unusable since it will not be recognized on the recipient's channel.
                botParty = RoutingDataManager.FindBotPartyByChannelAndConversation(
                    partyToMessage.ChannelId, partyToMessage.ConversationAccount);
            }

            if (botParty != null)
            {
                messageActivity.From = botParty.ChannelAccount;

                MessagingUtils.ConnectorClientAndMessageBundle bundle =
                    MessagingUtils.CreateConnectorClientAndMessageActivity(
                        partyToMessage.ServiceUrl, messageActivity);

                return await bundle.connectorClient.Conversations.SendToConversationAsync(
                    (Activity)bundle.messageActivity);
            }

            return null;
        }

        /// <summary>
        /// Tries to send the given message to the given party using this bot on the same channel
        /// as the party who the message is sent to.
        /// </summary>
        /// <param name="partyToMessage">The party to send the message to.</param>
        /// <param name="messageText">The message content.</param>
        /// <returns>The ResourceResponse instance or null in case of an error.</returns>
        public async Task<ResourceResponse> SendMessageToPartyByBotAsync(Party partyToMessage, string messageText, string eventText = null)
        {
            Party botParty = null;

            if (partyToMessage != null)
            {
                botParty = RoutingDataManager.FindBotPartyByChannelAndConversation(
                    partyToMessage.ChannelId, partyToMessage.ConversationAccount);
            }

            if (botParty != null)
            {
                MessagingUtils.ConnectorClientAndMessageBundle bundle =
                    MessagingUtils.CreateConnectorClientAndMessageActivity(
                        partyToMessage, messageText, botParty?.ChannelAccount, eventText);

                return await bundle.connectorClient.Conversations.SendToConversationAsync(
                    (Activity)bundle.messageActivity);
            }

            return null;
        }

        /// <summary>
        /// Handles the new activity.
        /// </summary>
        /// <param name="activity">The activity to handle.</param>
        /// <param name="tryToInitiateEngagementIfNotEngaged">If true, will try to initiate
        /// the engagement (1:1 conversation) automatically, if the sender is not engaged already.</param>
        /// <param name="addClientNameToMessage">If true, will add the client's name to the beginning of the message.</param>
        /// <param name="addOwnerNameToMessage">If true, will add the owner's (agent) name to the beginning of the message.</param>
        /// <returns>The result of the operation.</returns>        
        public async Task<MessageRouterResult> HandleActivityAsync(
            Activity activity, bool addClientNameToMessage = true, bool addOwnerNameToMessage = false)
        {
            MessageRouterResult result = new MessageRouterResult()
            {
                Type = MessageRouterResultType.NoActionTaken
            };

            // No command to the bot was issued so it must be an actual message then

            result = await HandleMessageAsync(activity, addClientNameToMessage, addOwnerNameToMessage);

            if (result.Type == MessageRouterResultType.NoActionTaken)
            {
                // Check for aggregation states
                Party senderParty = MessagingUtils.CreateSenderParty(activity);

                if (RoutingDataManager.IsPendingRequest(senderParty))
                {
                    result.Type = MessageRouterResultType.ConnectionAlreadyRequested;
                }
                else if (senderParty.AllowBackchannel)
                {
                    result.Type = MessageRouterResultType.FailedToForwardMessage;
                }
            }

            return result;
        }
        /// <summary>
        /// Check unused conversation to delete
        /// </summary>
        /// <returns></returns>
        public async Task CheckConversationStatesAsync()
        {
            DateTimeOffset maxDate = DateTime.UtcNow.AddSeconds(-AeccStrings.Timeout_Engagement_InSg);
            var partiesToRemove = RoutingDataManager.GetUserPartiesWithoutActivity(maxDate);
            foreach (var party in partiesToRemove)
            {
                RoutingDataManager.RemoveParty(party);
            }

            maxDate = DateTime.UtcNow.AddSeconds(-AeccStrings.Timeout_AggregationChecked_InSg);
            var aggregationsToRemove = RoutingDataManager.GetAggregationsWithoutActivity(maxDate);
            foreach (var party in aggregationsToRemove)
            {
                RoutingDataManager.RemoveAggregationParty(party);
            }

            maxDate = DateTime.UtcNow.AddSeconds(-AeccStrings.Timeout_PendingRequest_InSg);
            var pendingRequestsToRemove = RoutingDataManager.GetPendingRequestsWithoutActivity(maxDate);
            foreach (var party in pendingRequestsToRemove)
            {
                if (RoutingDataManager.RemovePendingRequest(party))
                {
                    string messageText = AeccStrings.EngagementInitiated_TimeoutClientText;
                    string eventText = null;
                    if (party.AllowBackchannel)
                    {
                        eventText = BackChannelCommands.GetMessageRouter(MessageRouterResultType.ConnectionRejected);
                    }
                    await SendMessageToPartyByBotAsync(party, messageText, eventText);
                }
            }
        }

        /// <summary>
        /// Checks the given parties and adds them to the collection, if not already there.
        /// 
        /// Note that this method expects that the recipient is the bot. The sender could also be
        /// the bot, but that case is checked before adding the sender to the container.
        /// </summary>
        /// <param name="senderParty">The sender party (from).</param>
        /// <param name="recipientParty">The recipient party.</param>
        public void MakeSurePartiesAreTracked(Party senderParty, Party recipientParty, bool allowBackchannel = false)
        {
            // Check that the party who sent the message is not the bot
            if (!RoutingDataManager.IsBot(senderParty))
            {
                // Store the user party, if not already stored
                RoutingDataManager.AddParty(senderParty);
            }
            // Store the bot identity, if not already stored
            RoutingDataManager.AddParty(recipientParty, false);

            RoutingDataManager.UpdateUserPartyActivity(senderParty, allowBackchannel);
        }

        /// <summary>
        /// Checks the given activity for new parties and adds them to the collection, if not
        /// already there.
        /// </summary>
        /// <param name="activity">The activity.</param>
        public void MakeSurePartiesAreTracked(IActivity activity, bool allowBackchannel = false)
        {
            MakeSurePartiesAreTracked(
                MessagingUtils.CreateSenderParty(activity),
                MessagingUtils.CreateRecipientParty(activity), allowBackchannel);
        }

        /// <summary>
        /// Removes the given party from the routing data.
        /// </summary>
        /// <param name="partyToRemove">The party to remove.</param>
        /// <returns>The results. If the number of results is more than 0, the operation was successful.</returns>
        public IList<MessageRouterResult> RemoveParty(Party partyToRemove)
        {
            IList<MessageRouterResult> messageRouterResults = RoutingDataManager.RemoveParty(partyToRemove);
            return messageRouterResults;
        }

        #region Connection Methods
        /// <summary>
        /// Tries to initiate a connection (1:1 conversation) by creating a request on behalf of
        /// the sender in the given activity. This method does nothing, if a request for the same
        /// user already exists.
        /// </summary>
        /// <param name="activity">The activity.</param>
        /// <returns>The result of the operation.</returns>
        public MessageRouterResult RequestConnection(Activity activity, string selectedOption)
        {
            MessageRouterResult messageRouterResult =
                RoutingDataManager.AddPendingRequest(MessagingUtils.CreateSenderParty(activity));
            messageRouterResult.Activity = activity;
            messageRouterResult.SelectedOption = selectedOption;
            return messageRouterResult;
        }


        /// <summary>
        /// Tries to reject the pending engagement request of the given party.
        /// </summary>
        /// <param name="partyToReject">The party whose request to reject.</param>
        /// <param name="rejecterParty">The party rejecting the request (optional).</param>
        /// <returns>The result of the operation.</returns>
        public MessageRouterResult RejectPendingRequest(Party partyToReject, Party rejecterParty = null)
        {
            if (partyToReject == null)
            {
                throw new ArgumentNullException($"The party to reject ({nameof(partyToReject)} cannot be null");
            }

            MessageRouterResult result = new MessageRouterResult()
            {
                ConversationOwnerParty = rejecterParty,
                ConversationClientParty = partyToReject
            };

            if (RoutingDataManager.RemovePendingRequest(partyToReject))
            {
                result.Type = MessageRouterResultType.ConnectionRejected;
            }
            else
            {
                result.Type = MessageRouterResultType.Error;
                result.ErrorMessage = $"Fallo al rechazar petición pendiente del usuario \"{partyToReject.ChannelAccount?.Name}\"";
            }
            
            return result;
        }

        /// <summary>
        /// Tries to establish 1:1 chat between the two given parties.
        /// Note that the conversation owner will have a new separate party in the created engagement.
        /// </summary>
        /// <param name="conversationOwnerParty">The party who owns the conversation (e.g. customer service agent).</param>
        /// <param name="conversationClientParty">The other party in the conversation.</param>
        /// <param name="createNewDirectConversation">If true, will try to create a new direct conversation between
        /// the bot and the conversation owner (e.g. agent) where the messages from the other (client) party are routed.
        /// Note that this will result in the conversation owner having a new separate party in the created connection
        /// (for the new direct conversation).</param>
        /// <returns>The result of the operation.</returns>
        public async Task<MessageRouterResult> ConnectAsync(
            Party conversationOwnerParty, Party conversationClientParty, bool createNewDirectConversation)
        {
            if (conversationOwnerParty == null || conversationClientParty == null)
            {
                throw new ArgumentNullException(
                    $"Neither of the arguments ({nameof(conversationOwnerParty)}, {nameof(conversationClientParty)}) can be null");
            }

            MessageRouterResult result = new MessageRouterResult()
            {
                ConversationOwnerParty = conversationOwnerParty,
                ConversationClientParty = conversationClientParty
            };

            Party acceptorPartyEngaged = conversationOwnerParty;
            Party botParty = RoutingDataManager.FindBotPartyByChannelAndConversation(
                conversationOwnerParty.ChannelId, conversationOwnerParty.ConversationAccount);

            if (botParty != null)
            {
                if (createNewDirectConversation)
                {
                    ConnectorClient connectorClient = new ConnectorClient(new Uri(conversationOwnerParty.ServiceUrl));
                    ConversationResourceResponse conversationResourceResponse = null;

                    try
                    {
                        conversationResourceResponse = await connectorClient.Conversations.CreateDirectConversationAsync(
                           botParty.ChannelAccount, conversationOwnerParty.ChannelAccount);
                    }
                    catch (Exception)
                    {
                        // Do nothing here as we fallback (continue without creating a direct conversation)
                    }

                    if (conversationResourceResponse != null && !string.IsNullOrEmpty(conversationResourceResponse.Id))
                    {
                        // The conversation account of the conversation owner for this 1:1 chat is different -
                        // thus, we need to create a new party instance
                        ConversationAccount directConversationAccount = new ConversationAccount(id: conversationResourceResponse.Id);

                        acceptorPartyEngaged = new PartyWithTimestamps(
                            conversationOwnerParty.ServiceUrl, conversationOwnerParty.ChannelId,
                            conversationOwnerParty.ChannelAccount, directConversationAccount);

                        RoutingDataManager.AddParty(acceptorPartyEngaged);
                        RoutingDataManager.AddParty(new PartyWithTimestamps(
                            botParty.ServiceUrl, botParty.ChannelId, botParty.ChannelAccount, directConversationAccount), false);

                        result.ConversationResourceResponse = conversationResourceResponse;
                    }
                }

                result = RoutingDataManager.ConnectAndClearPendingRequest(acceptorPartyEngaged, conversationClientParty);

                if (result.Type != MessageRouterResultType.Error
                    && RoutingDataManager.IsInAggregation(conversationOwnerParty))
                {
                    RoutingDataManager.RemoveAggregationParty(conversationOwnerParty);
                }
            }
            else
            {
                result.Type = MessageRouterResultType.Error;
                result.ErrorMessage = "Failed to find the bot instance";
            }
            
            return result;
        }

        /// <summary>
        /// Ends the 1:1 conversation where the given party is the conversation owner
        /// (e.g. a customer service agent).
        /// </summary>
        /// <param name="conversationOwnerParty">The owner of the connection (conversation).</param>
        /// <returns>The results.</returns>
        public List<MessageRouterResult> Disconnect(Party conversationOwnerParty)
        {
            List<MessageRouterResult> messageRouterResults = new List<MessageRouterResult>();

            Party ownerInConversation = RoutingDataManager.FindEngagedPartyByChannel(
                conversationOwnerParty.ChannelId, conversationOwnerParty.ChannelAccount);

            if (ownerInConversation != null)
            {
                messageRouterResults.AddRange(
                    RoutingDataManager.Disconnect(ownerInConversation, ConnectionProfile.Any));

                if (messageRouterResults.Count==1)
                {
                    RoutingDataManager.AddAggregationParty(messageRouterResults.First().ConversationOwnerParty);
                }
            }
            else if (RoutingDataManager.IsPendingRequest(conversationOwnerParty))
            {
                if (RoutingDataManager.RemovePendingRequest(conversationOwnerParty))
                {
                    messageRouterResults.Add(new MessageRouterResult()
                    {
                        Type = MessageRouterResultType.OK
                    });
                }
            }
            else
            {
                messageRouterResults.Add(new MessageRouterResult()
                {
                    Type = MessageRouterResultType.Error,
                    ConversationOwnerParty = conversationOwnerParty,
                    ErrorMessage = AeccStrings.EndEngagement_NotFoundText
                });
            }


            return messageRouterResults;
        }
        #endregion

       


        /// <summary>
        /// Handles the incoming message activities. For instance, if it is a message from party
        /// engaged in a chat, the message will be forwarded to the counterpart in whatever
        /// channel that party is on.
        /// </summary>
        /// <param name="activity">The activity to handle.</param>
        /// <param name="addClientNameToMessage">If true, will add the client's name to the beginning of the message.</param>
        /// <param name="addOwnerNameToMessage">If true, will add the owner's (agent) name to the beginning of the message.</param>
        /// <returns>The result of the operation.</returns>
        private async Task<MessageRouterResult> HandleMessageAsync(Activity activity, bool addClientNameToMessage = true, bool addOwnerNameToMessage = false)
        {
            MessageRouterResult result = new MessageRouterResult()
            {
                Type = MessageRouterResultType.NoActionTaken,
                Activity = activity
            };

            Party senderParty = MessagingUtils.CreateSenderParty(activity);

            if (RoutingDataManager.IsConnected(senderParty, ConnectionProfile.Owner))
            {
                // Sender is an owner of an ongoing conversation - forward the message
                result.ConversationOwnerParty = senderParty;
                Party partyToForwardMessageTo = RoutingDataManager.GetConnectedCounterpart(senderParty);

                if (partyToForwardMessageTo != null)
                {
                    result.ConversationClientParty = partyToForwardMessageTo;
                    string message = addOwnerNameToMessage
                        ? $"{senderParty.ChannelAccount.Name}: {activity.Text}" : activity.Text;
                    ResourceResponse resourceResponse =
                        await SendMessageToPartyByBotAsync(partyToForwardMessageTo, activity.Text);

                    if (MessagingUtils.WasSuccessful(resourceResponse))
                    {
                        result.Type = MessageRouterResultType.OK;
                    }
                    else
                    {
                        result.Type = MessageRouterResultType.FailedToForwardMessage;
                        result.ErrorMessage = $"No se pudo mandar el mensaje al usuario {partyToForwardMessageTo}";
                    }
                }
                else
                {
                    result.Type = MessageRouterResultType.FailedToForwardMessage;
                    result.ErrorMessage = "No se ha podido encontrar el grupo para reenviar el mensaje";
                }
            }
            else if (RoutingDataManager.IsConnected(senderParty, ConnectionProfile.Client))
            {
                // Sender is a participant of an ongoing conversation - forward the message
                result.ConversationClientParty = senderParty;
                Party partyToForwardMessageTo = RoutingDataManager.GetConnectedCounterpart(senderParty);

                if (partyToForwardMessageTo != null)
                {
                    result.ConversationOwnerParty = partyToForwardMessageTo;
                    string message = addClientNameToMessage ?
                        $"{senderParty.ChannelAccount.Name}: {activity.Text}" : activity.Text;
                    await SendMessageToPartyByBotAsync(partyToForwardMessageTo, message);
                    result.Type = MessageRouterResultType.OK;
                }
                else
                {
                    result.Type = MessageRouterResultType.FailedToForwardMessage;
                    result.ErrorMessage = "No se ha podido encontrar el grupo para reenviar el mensaje";
                }
            }
            else if (RoutingDataManager.IsInAggregation(senderParty))
            {
                await SendMessageToPartyByBotAsync(senderParty, AeccStrings.SendText_AlreadyAggregation);
                result.Type = MessageRouterResultType.OK;
            }
            
            return result;
        }
    }
}