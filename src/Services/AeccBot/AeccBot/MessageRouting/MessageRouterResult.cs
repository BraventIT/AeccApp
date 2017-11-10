﻿using Aecc.Models;
using Microsoft.Bot.Connector;
using System.Collections.Generic;
using System.Linq;

namespace AeccBot.MessageRouting
{
    /// <summary>
    /// Represents a result of more complex operations executed by MessageRouterManager (when
    /// boolean just isn't enough).
    /// 
    /// Note that - as this class serves different kind of operations with different kind of
    /// outcomes - some of the properties can be null. The type of the result defines which
    /// properties are meaningful.
    /// </summary>
    public class MessageRouterResult
    {
        public MessageRouterResultType Type
        {
            get;
            set;
        }

        /// <summary>
        /// Activity instance associated with the result.
        /// </summary>        
        public Activity Activity{  get;set;}


        public string SelectedOption {get;set; }

        /// <summary>
        /// A valid ConversationResourceResponse of the newly created direct conversation
        /// (between the bot [who will relay messages] and the conversation owner),
        /// if the engagement was added and a conversation created successfully
        /// (MessageRouterResultType is EngagementAdded).
        /// </summary>
        public ConversationResourceResponse ConversationResourceResponse
        {
            get;
            set;
        }

        public Party ConversationOwnerParty
        {
            get;
            set;
        }

        public Party ConversationClientParty
        {
            get;
            set;
        }

        public string ErrorMessage
        {
            get;
            set;
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public MessageRouterResult()
        {
            ErrorMessage = string.Empty;
        }

        public override string ToString()
        {
            return $"[{Type}; {ConversationResourceResponse}; {ConversationOwnerParty}; {ConversationClientParty}; {ErrorMessage}]";
        }

        public static bool IsOk(IList<MessageRouterResult> messageRouterResults)
        {
            if (messageRouterResults==null || !messageRouterResults.Any())
            {
                return false;
            }

            return !messageRouterResults.Any(m => m.Type == MessageRouterResultType.Error);
        }
    }
}