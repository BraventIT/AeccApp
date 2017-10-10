using AeccApp.Core.Models;
using AeccBot.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AeccApp.Core.Services
{
    public interface IChatService
    {
        /// <summary>
        /// Is into conversation
        /// </summary>
        bool InConversation { get; }

        /// <summary>
        /// Counterpart
        /// </summary>
        UserData ConversationCounterpart { get; }

        #region Only Volunteers Properties
        /// <summary>
        /// Is volunteer active (online)
        /// </summary>
        bool VolunteerIsActive { get; }
        #endregion

        /// <summary>
        /// Initialize service. 
        /// </summary>
        Task InitializeAsync();

        /// <summary>
        /// Send text message in current conversation
        /// </summary>
        /// <param name="messageText">message to send</param>
        Task SendMessageAsync(string messageText);

        /// <summary>
        /// Raised when a new text message is received
        /// </summary>
        event EventHandler<IList<Message>> MessagesReceived;

        /// <summary>
        /// Raised when a new volunteer is active
        /// </summary>
        event EventHandler<IList<Volunteer>> AggregationsReceived;

        /// <summary>
        /// Get volunteers active list
        /// </summary>
        /// <returns></returns>
        Task<IList<Volunteer>> GetListVolunteersAsync();

        /// <summary>
        /// Try to initialize new chat conversation with selected volunteer
        /// </summary>
        /// <param name="partyId">Party identity that represent a valunteer connection</param>
        /// <returns></returns>
        Task<bool> InitializeChatAsync(string partyId);

        /// <summary>
        /// End current chat conversation
        /// </summary>
        /// <returns>Is ok</returns>
        Task<bool> EndChatAsync();

        /// <summary>
        /// Get all messages of current conversation
        /// </summary>
        /// <returns></returns>
        IList<Message> GetConversationMessages();

        #region Only Volunteers Methods
        /// <summary>
        /// Change volunteer state (is online)
        /// </summary>
        /// <param name="isActive">Is active</param>
        /// <returns></returns>
        Task<bool> SetVolunteerState(bool isActive);

        /// <summary>
        /// Accept or reject a beneficiary request
        /// </summary>
        /// <param name="doAccept">Accept or reject</param>
        /// <param name="partyId">Party identity that represent a beneficiary conection</param>
        /// <returns></returns>
        Task<bool> AcceptAndRejectRequestAsync(bool doAccept, string partyId);
        #endregion
    }
}
