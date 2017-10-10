using AeccBot.Models;
using Microsoft.Bot.Connector;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace AeccBot.MessageRouting.DataStore
{
    /// <summary>
    /// Routing data manager that stores the data locally.
    /// 
    /// NOTE: USE THIS CLASS ONLY FOR TESTING! Storing the data like this in production would
    /// not work since the bot may have multiple instances.
    /// 
    /// See IRoutingDataManager for general documentation of properties and methods.
    /// </summary>
    [Serializable]
    public class LocalRoutingDataManager : IRoutingDataManager
    {
        /// <summary>
        /// Parties that are users (not this bot).
        /// </summary>
        protected IList<Party> UserParties {get; set;}

        /// <summary>
        /// If the bot is addressed from different channels, its identity in terms of ID and name
        /// can vary. Those different identities are stored in this list.
        /// </summary>
        protected IList<Party> BotParties{ get;set;}

        /// <summary>
        /// Represents the channels (and the specific conversations e.g. specific channel in Slack),
        /// where the chat requests are directed. For instance, a channel could be where the
        /// customer service agents accept customer chat requests. 
        /// </summary>
        protected IList<Party> AggregationParties {get;set;}

        /// <summary>
        /// The list of parties waiting for their (conversation) requests to be accepted.
        /// </summary>
        protected List<Party> PendingRequests{ get; set; }

        /// <summary>
        /// Contains 1:1 associations between parties i.e. parties engaged in a conversation.
        /// Furthermore, the key party is considered to be the conversation owner e.g. in
        /// a customer service situation the customer service agent.
        /// </summary>
        protected Dictionary<Party, Party> ConnectedParties
        {
            get;
            set;
        }

#if DEBUG
        protected IList<MessageRouterResult> LastMessageRouterResults{get;set;}
#endif

        /// <summary>
        /// Constructor.
        /// </summary>
        public LocalRoutingDataManager()
        {
            AggregationParties = new List<Party>();
            UserParties = new List<Party>();
            BotParties = new List<Party>();
            PendingRequests = new List<Party>();
            ConnectedParties = new Dictionary<Party, Party>();
#if DEBUG
            LastMessageRouterResults = new List<MessageRouterResult>();
#endif
        }

        public virtual IList<Party> GetUserParties()
        {
            List<Party> userPartiesAsList = UserParties as List<Party>;
            return userPartiesAsList?.AsReadOnly();
        }

        public virtual IList<Party> GetBotParties()
        {
            List<Party> botPartiesAsList = BotParties as List<Party>;
            return botPartiesAsList?.AsReadOnly();
        }

        public virtual IList<Party> GetUserPartiesWithoutActivity(DateTimeOffset maxDate)
        {
            var userWithoutActivity = GetUserParties().Where(x => x.LastActivity < maxDate).ToList();
            return userWithoutActivity;
        }

        public virtual IList<Party> GetPendingRequestsWithoutActivity(DateTimeOffset maxDate)
        {
            var userWithoutActivity = GetPendingRequests().Where(x => (x as PartyWithTimestamps)?.ConnectionRequestTime < maxDate).ToList();
            return userWithoutActivity;
        }

        public virtual IList<Party> GetAggregationsWithoutActivity(DateTimeOffset maxDate)
        {
            var userWithoutActivity = GetAggregationParties().Where(x =>
                x.AllowBackchannel == true &&
                (x as PartyWithTimestamps)?.AggregationLastCheckedTime < maxDate).ToList();
            return userWithoutActivity;
        }


        public virtual void UpdateUserPartyActivity(Party party, bool allowBackchannel = false)
        {
            if (UserParties.Contains(party))
            {
                party.LastActivity = DateTime.UtcNow;
                party.AllowBackchannel |= allowBackchannel;
            }
        }

        public void UpdatePartyConnectionData(Party party, IActivity activity)
        {
            if (party.ServiceUrl != activity.ServiceUrl ||
                party.ChannelId != activity.ChannelId ||
                party.ConversationAccount.Id != activity.Conversation.Id)
            {
                // first fine current bot party
                var botParty = FindBotPartyByChannelAndConversation(party.ChannelId, party.ConversationAccount);

                // Refresh connection data
                party.ServiceUrl = activity.ServiceUrl;
                party.ChannelId = activity.ChannelId;
                party.ConversationAccount = activity.Conversation;

                botParty.ChannelId = activity.ChannelId;
                botParty.ConversationAccount = activity.Conversation;
            }
        }

        public virtual bool AddParty(Party newParty, bool isUser = true)
        {
            if (newParty == null || (isUser ? UserParties.Contains(newParty) : BotParties.Contains(newParty)))
            {
                return false;
            }

            if (isUser)
            {
                UserParties.Add(newParty);
            }
            else
            {
                if (newParty.ChannelAccount == null)
                {
                    throw new NullReferenceException($"Channel account of a bot party ({nameof(newParty.ChannelAccount)}) cannot be null");
                }

                BotParties.Add(newParty);
            }

            return true;
        }

        public virtual bool AddParty(string serviceUrl, string channelId,
            ChannelAccount channelAccount, ConversationAccount conversationAccount,
            bool isUser = true)
        {
            Party newParty = new PartyWithTimestamps(serviceUrl, channelId, channelAccount, conversationAccount);
            return AddParty(newParty, isUser);
        }

        public virtual IList<MessageRouterResult> RemoveParty(Party partyToRemove)
        {
            List<MessageRouterResult> messageRouterResults = new List<MessageRouterResult>();
            bool wasRemoved = false;

            // Check user and bot parties
            IList<Party>[] partyLists = new IList<Party>[]
            {
                UserParties,
                BotParties
            };

            foreach (IList<Party> partyList in partyLists)
            {
                IList<Party> partiesToRemove = FindPartiesWithMatchingChannelAccount(partyToRemove, partyList);

                if (partiesToRemove != null)
                {
                    foreach (Party party in partiesToRemove)
                    {
                        if (partyList.Remove(party))
                        {
                            wasRemoved = true;
                        }
                    }
                }
            }

            // Check pending requests
            IList<Party> pendingRequestsToRemove = FindPartiesWithMatchingChannelAccount(partyToRemove, PendingRequests);

            foreach (Party pendingRequestToRemove in pendingRequestsToRemove)
            {
                if (PendingRequests.Remove(pendingRequestToRemove))
                {
                    wasRemoved = true;

                    messageRouterResults.Add(new MessageRouterResult()
                    {
                        Type = MessageRouterResultType.ConnectionRejected,
                        ConversationClientParty = pendingRequestToRemove
                    });
                }
            }

            if (wasRemoved)
            {
                // Check if the party exists in EngagedParties
                List<Party> keys = new List<Party>();

                foreach (var partyPair in ConnectedParties)
                {
                    if (partyPair.Key.HasMatchingChannelInformation(partyToRemove)
                        || partyPair.Value.HasMatchingChannelInformation(partyToRemove))
                    {
                        keys.Add(partyPair.Key);
                    }
                }

                foreach (Party key in keys)
                {
                    messageRouterResults.AddRange(Disconnect(key, ConnectionProfile.Owner));
                }
            }

            return messageRouterResults;
        }

        public virtual IList<Party> GetAggregationParties()
        {
            List<Party> aggregationPartiesAsList = AggregationParties as List<Party>;
            return aggregationPartiesAsList?.AsReadOnly();
        }

        public virtual bool AddAggregationParty(Party party)
        {
            if (party != null && !AggregationParties.Contains(party))
            {
                if (party is PartyWithTimestamps)
                {
                    (party as PartyWithTimestamps).AggregationLastCheckedTime = DateTime.UtcNow;
                }

                AggregationParties.Add(party);
                return true;
            }
            return false;
        }

        public virtual bool RemoveAggregationParty(Party party)
        {
            if (party is PartyWithTimestamps)
            {
                (party as PartyWithTimestamps).ResetAggregationLastCheckedTime();
            }
            return AggregationParties.Remove(party);
        }

        public virtual bool UpdateCheckAggregationParty(Party party)
        {
            if (party != null && AggregationParties.Contains(party))
            {
                if (party is PartyWithTimestamps)
                {
                    (party as PartyWithTimestamps).AggregationLastCheckedTime = DateTime.UtcNow;
                }
                return true;
            }
            return false;
        }

        public bool IsInAggregation(Party party)
        {
            return AggregationParties.Contains(party);
        }

        public bool IsBot(Party party)
        {
            return BotParties.Contains(party);
        }

        public virtual IList<Party> GetPendingRequests()
        {
            List<Party> pendingRequestsAsList = PendingRequests as List<Party>;
            return pendingRequestsAsList?.AsReadOnly();
        }

        public virtual MessageRouterResult AddPendingRequest(Party party)
        {
            MessageRouterResult result = new MessageRouterResult()
            {
                ConversationClientParty = party
            };

            if (party != null)
            {
                if (PendingRequests.Contains(party))
                {
                    result.Type = MessageRouterResultType.ConnectionAlreadyRequested;
                }
                else
                {
                    if (!AggregationParties.Any())
                    {
                        result.Type = MessageRouterResultType.NoAgentsAvailable;
                    }
                    else
                    {
                        if (party is PartyWithTimestamps)
                        {
                            (party as PartyWithTimestamps).ConnectionRequestTime = DateTime.UtcNow;
                        }

                        PendingRequests.Add(party);
                        result.Type = MessageRouterResultType.ConnectionRequested;
                    }
                }
            }
            else
            {
                result.Type = MessageRouterResultType.Error;
                result.ErrorMessage = "The given party instance is null";
            }

            return result;
        }

        public virtual bool RemovePendingRequest(Party party)
        {
            if (party is PartyWithTimestamps)
            {
                (party as PartyWithTimestamps).ResetConnectionRequestTime();
            }

            return PendingRequests.Remove(party);
        }

        public bool IsPendingRequest(Party party)
        {
            return PendingRequests.Contains(party);
        }

        public virtual bool IsConnected(Party party, ConnectionProfile engagementProfile)
        {
            bool isEngaged = false;

            if (party != null)
            {
                switch (engagementProfile)
                {
                    case ConnectionProfile.Client:
                        isEngaged = ConnectedParties.Values.Contains(party);
                        break;
                    case ConnectionProfile.Owner:
                        isEngaged = ConnectedParties.Keys.Contains(party);
                        break;
                    case ConnectionProfile.Any:
                        isEngaged = (ConnectedParties.Values.Contains(party) || ConnectedParties.Keys.Contains(party));
                        break;
                    default:
                        break;
                }
            }

            return isEngaged;
        }

        public virtual Party GetConnectedCounterpart(Party partyWhoseCounterpartToFind)
        {
            Party counterparty = null;

            if (IsConnected(partyWhoseCounterpartToFind, ConnectionProfile.Client))
            {
                for (int i = 0; i < ConnectedParties.Count; ++i)
                {
                    if (ConnectedParties.Values.ElementAt(i).Equals(partyWhoseCounterpartToFind))
                    {
                        counterparty = ConnectedParties.Keys.ElementAt(i);
                        break;
                    }
                }
            }
            else if (IsConnected(partyWhoseCounterpartToFind, ConnectionProfile.Owner))
            {
                ConnectedParties.TryGetValue(partyWhoseCounterpartToFind, out counterparty);
            }

            return counterparty;
        }

        public virtual MessageRouterResult ConnectAndClearPendingRequest(Party conversationOwnerParty, Party conversationClientParty)
        {
            MessageRouterResult result = new MessageRouterResult()
            {
                ConversationOwnerParty = conversationOwnerParty,
                ConversationClientParty = conversationClientParty
            };

            if (conversationOwnerParty != null && conversationClientParty != null)
            {
                try
                {
                    ConnectedParties.Add(conversationOwnerParty, conversationClientParty);
                    PendingRequests.Remove(conversationClientParty);

                    DateTime connectionStartedTime = DateTime.UtcNow;

                    if (conversationClientParty is PartyWithTimestamps)
                    {
                        (conversationClientParty as PartyWithTimestamps).ResetConnectionRequestTime();
                        (conversationClientParty as PartyWithTimestamps).ConnectionEstablishedTime = connectionStartedTime;
                    }

                    if (conversationOwnerParty is PartyWithTimestamps)
                    {
                        (conversationOwnerParty as PartyWithTimestamps).ConnectionEstablishedTime = connectionStartedTime;
                    }

                    result.Type = MessageRouterResultType.Connected;
                }
                catch (ArgumentException e)
                {
                    result.Type = MessageRouterResultType.Error;
                    result.ErrorMessage = e.Message;
                    Debug.WriteLine($"Failed to add engagement between parties {conversationOwnerParty} and {conversationClientParty}: {e.Message}");
                }
            }
            else
            {
                result.Type = MessageRouterResultType.Error;
                result.ErrorMessage = "Either the owner or the client is missing";
            }

            return result;
        }

        public virtual IList<MessageRouterResult> Disconnect(Party party, ConnectionProfile engagementProfile)
        {
            IList<MessageRouterResult> messageRouterResults = new List<MessageRouterResult>();

            if (party != null)
            {
                List<Party> keysToRemove = new List<Party>();

                foreach (var partyPair in ConnectedParties)
                {
                    bool removeThisPair = false;

                    switch (engagementProfile)
                    {
                        case ConnectionProfile.Client:
                            removeThisPair = partyPair.Value.Equals(party);
                            break;
                        case ConnectionProfile.Owner:
                            removeThisPair = partyPair.Key.Equals(party);
                            break;
                        case ConnectionProfile.Any:
                            removeThisPair = (partyPair.Value.Equals(party) || partyPair.Key.Equals(party));
                            break;
                        default:
                            break;
                    }

                    if (removeThisPair)
                    {
                        keysToRemove.Add(partyPair.Key);

                        if (engagementProfile == ConnectionProfile.Owner)
                        {
                            // Since owner is the key in the dictionary, there can be only one
                            break;
                        }
                    }
                }

                messageRouterResults = RemoveConnections(keysToRemove);
            }

            return messageRouterResults;
        }

        public virtual void DeleteAll()
        {
            AggregationParties.Clear();
            UserParties.Clear();
            BotParties.Clear();
            PendingRequests.Clear();
            ConnectedParties.Clear();
#if DEBUG
            LastMessageRouterResults.Clear();
#endif
        }

        public virtual bool IsAssociatedWithAggregation(Party party)
        {
            return (party != null && AggregationParties != null && AggregationParties.Count() > 0
                    && AggregationParties.Where(aggregationParty =>
                        aggregationParty.ConversationAccount.Id == party.ConversationAccount.Id
                        && aggregationParty.ServiceUrl == party.ServiceUrl
                        && aggregationParty.ChannelId == party.ChannelId).Count() > 0);
        }

        public virtual string ResolveBotNameInConversation(Party party)
        {
            string botName = null;

            if (party != null)
            {
                Party botParty = FindBotPartyByChannelAndConversation(party.ChannelId, party.ConversationAccount);

                if (botParty != null && botParty.ChannelAccount != null)
                {
                    botName = botParty.ChannelAccount.Name;
                }
            }

            return botName;
        }

        public virtual Party FindExistingUserParty(Party partyToFind)
        {
            Party foundParty = null;

            try
            {
                foundParty = UserParties.First(party => partyToFind.Equals(party));
            }
            catch (ArgumentNullException)
            {
            }
            catch (InvalidOperationException)
            {
            }

            return foundParty;
        }

        public virtual Party FindPartyByChannelAccountIdAndConversationId(string channelAccountId, string conversationId)
        {
            Party userParty = null;

            try
            {
                userParty = UserParties.Single(party =>
                        (party.ChannelAccount.Id.Equals(channelAccountId)
                         && party.ConversationAccount.Id.Equals(conversationId)));
            }
            catch (InvalidOperationException)
            {
            }

            return userParty;
        }

        public virtual Party FindBotPartyByChannelAndConversation(string channelId, ConversationAccount conversationAccount)
        {
            Party botParty = null;

            try
            {
                botParty = BotParties.Single(party =>
                        (party.ChannelId.Equals(channelId)
                         && party.ConversationAccount.Id.Equals(conversationAccount.Id)));
            }
            catch (InvalidOperationException)
            {
            }

            return botParty;
        }

        public virtual Party FindEngagedPartyByChannel(string channelId, ChannelAccount channelAccount)
        {
            Party foundParty = null;

            foundParty = ConnectedParties.Keys.FirstOrDefault(party =>
                    (party.ChannelId.Equals(channelId)
                     && party.ChannelAccount != null
                     && party.ChannelAccount.Id.Equals(channelAccount.Id)));

            if (foundParty == null)
            {
                // Not found in keys, try the values
                foundParty = ConnectedParties.Values.FirstOrDefault(party =>
                        (party.ChannelId.Equals(channelId)
                         && party.ChannelAccount != null
                         && party.ChannelAccount.Id.Equals(channelAccount.Id)));
            }

            return foundParty;
        }

        public virtual IList<Party> FindPartiesWithMatchingChannelAccount(Party partyToFind, IList<Party> parties)
        {
            IList<Party> matchingParties = null;
            IEnumerable<Party> foundParties = null;

            try
            {
                foundParties = UserParties.Where(party => party.HasMatchingChannelInformation(partyToFind));
            }
            catch (ArgumentNullException)
            {
            }
            catch (InvalidOperationException)
            {
            }

            if (foundParties != null)
            {
                matchingParties = foundParties.ToArray();
            }

            return matchingParties;
        }

       

        /// <summary>
        /// Removes the connections of the given conversation owners.
        /// </summary>
        /// <param name="conversationOwnerParties">The conversation owners whose connections to remove.</param>
        /// <returns>The number of connections removed.</returns>
        protected virtual IList<MessageRouterResult> RemoveConnections(IList<Party> conversationOwnerParties)
        {
            IList<MessageRouterResult> messageRouterResults = new List<MessageRouterResult>();

            foreach (Party conversationOwnerParty in conversationOwnerParties)
            {
                ConnectedParties.TryGetValue(conversationOwnerParty, out Party conversationClientParty);

                if (ConnectedParties.Remove(conversationOwnerParty))
                {
                    if (conversationOwnerParty is PartyWithTimestamps)
                    {
                        (conversationOwnerParty as PartyWithTimestamps).ResetConnectionEstablishedTime();
                    }

                    if (conversationClientParty is PartyWithTimestamps)
                    {
                        (conversationClientParty as PartyWithTimestamps).ResetConnectionEstablishedTime();
                    }

                    messageRouterResults.Add(new MessageRouterResult()
                    {
                        Type = MessageRouterResultType.Disconnected,
                        ConversationOwnerParty = conversationOwnerParty,
                        ConversationClientParty = conversationClientParty
                    });
                }
            }

            return messageRouterResults;
        }

#if DEBUG
        /// <returns>The engagements (parties in conversation) as a string.</returns>
        public string ConnectionsToString()
        {
            string parties = string.Empty;

            foreach (KeyValuePair<Party, Party> keyValuePair in ConnectedParties)
            {
                parties += keyValuePair.Key + " -> " + keyValuePair.Value + "\n";
            }

            return parties;
        }

        public string GetLastMessageRouterResults()
        {
            string lastResultsAsString = string.Empty;

            foreach (MessageRouterResult result in LastMessageRouterResults)
            {
                lastResultsAsString += $"{result.ToString()}\n";
            }

            return lastResultsAsString;
        }

        public void AddMessageRouterResult(MessageRouterResult result)
        {
            if (result != null)
            {
                if (LastMessageRouterResults.Count > 9)
                {
                    LastMessageRouterResults.Remove(LastMessageRouterResults.ElementAt(0));
                }

                LastMessageRouterResults.Add(result);

                Debug.WriteLine($"Message router result: {result.ToString()}");
            }
        }

#endif
    }
}