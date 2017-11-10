﻿using Microsoft.Bot.Connector;
using System;
using Aecc.Models;

namespace AeccBot.Models
{
    /// <summary>
    /// Like Party, but with timestamps to mark times for when requests were made etc.
    /// </summary>
    [Serializable]
    public class PartyWithTimestamps : Party
    {
        /// <summary>
        /// Represents the time when a request was made.
        /// DateTime.MinValue will indicate that no request is pending.
        /// </summary>
        public DateTime ConnectionRequestTime
        {
            get;
            set;
        }

        /// <summary>
        /// Represents the time when the connection (1:1 conversation) was established.
        /// DateTime.MinValue will indicate that this party is not connected.
        /// </summary>
        public DateTime ConnectionEstablishedTime
        {
            get;
            set;
        }

        /// <summary>
        /// Represents the time when aggregation was checked.
        /// DateTime.MinValue will indicate that this party is not connected.
        /// </summary>
        public DateTime AggregationLastCheckedTime
        {
            get;
            set;
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public PartyWithTimestamps(string serviceUrl, string channelId,
            ChannelAccount channelAccount, ConversationAccount conversationAccount)
            : base(serviceUrl, channelId, channelAccount, conversationAccount)
        {
            ResetConnectionRequestTime();
            ResetConnectionEstablishedTime();
            ResetAggregationLastCheckedTime();
        }

        public void ResetConnectionRequestTime()
        {
            ConnectionRequestTime = DateTime.MinValue;
        }

        public void ResetConnectionEstablishedTime()
        {
            ConnectionEstablishedTime = DateTime.MinValue;
        }

        public void ResetAggregationLastCheckedTime()
        {
            AggregationLastCheckedTime = DateTime.MinValue;
        }
    }
}