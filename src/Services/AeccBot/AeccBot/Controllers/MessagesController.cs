using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using AeccBot.MessageRouting;
using AeccBot.Dialogs;
using System.Linq;
using System;
using AeccBot.Models;
using System.Diagnostics;
using AeccBot.Utils;
using AeccBot.CommandHandling;

namespace AeccBot
{
    [BotAuthentication]
    public class MessagesController : ApiController
    {
        private MessageRouterManager MessageRouterManager
        {
            get { return WebApiConfig.MessageRouterManager; }
        }
        
        private IMessageRouterResultHandler MessageRouterResultHandler
        {
            get { return WebApiConfig.MessageRouterResultHandler; }
        }

        // <summary>
        /// Handler for direct commands given to the bot.
        /// </summary>
        private IBotCommandHandler CommandHandler
        {
            get { return WebApiConfig.CommandMessageHandler; }
        }
        
        private IBotCommandHandler BackchannelHandler
        {
            get { return WebApiConfig.BackChannelMessageHandler; }
        }

        /// <summary>
        /// POST: api/Messages
        /// Receive a message from a user and reply to it
        /// </summary>
        public async Task<HttpResponseMessage> Post([FromBody]Activity activity)
        {
            await MessageRouterManager.CheckConversationStatesAsync();

            // Check for back channel messages
            if (activity.Type == ActivityTypes.Event)
            {
                MessageRouterManager.MakeSurePartiesAreTracked(activity, true);

                // Check for  back channel commands
                await BackchannelHandler.HandleCommandAsync(activity);
            }
            // Check for normal messages
            else if ((activity.Type == ActivityTypes.Message))
            {
                MessageRouterManager.MakeSurePartiesAreTracked(activity);

                // Check for possible commands
                if (!await CommandHandler.HandleCommandAsync(activity))
                {
                    // No valid typed command detected

                    // Let the message router manager instance handle the activity
                    var messageRouterResult = await MessageRouterManager.HandleActivityAsync(activity);

                    if (messageRouterResult.Type == MessageRouterResultType.NoActionTaken)
                    {
                        await Conversation.SendAsync(activity, () => new SelectAggregationDialog());
                        //probando autentificacion
                        //  await Conversation.SendAsync(activity, () => new RootDialog());
                    }
                    else
                    {
                        // Handle the result, if required
                        await MessageRouterResultHandler.HandleResultAsync(messageRouterResult);
                    }
                }
            }
            else
            {
                await HandleSystemMessageAsync(activity);
            }

            var response = Request.CreateResponse(HttpStatusCode.OK);
            return response;
        }

        private async Task HandleSystemMessageAsync(Activity message)
        {
            if (message.Type == ActivityTypes.DeleteUserData)
            {
                // Implement user deletion here
                // If we handle user deletion, return a real message
                Party senderParty = MessagingUtils.CreateSenderParty(message);

                if (MessageRouterManager.RemoveParty(senderParty).Any())
                {
                    var connector = new ConnectorClient(new Uri(message.ServiceUrl));
                    var dataRemovedMessage = message.CreateReply($"Los datos de {senderParty.ChannelAccount?.Name} se han borrado.");
                    await connector.Conversations.ReplyToActivityAsync(dataRemovedMessage);
                }
            }
            else if (message.Type == ActivityTypes.ConversationUpdate)
            {
                // Handle conversation state changes, like members being added and removed
                // Not available in all channels

                if (message.MembersAdded.Any() && message.MembersAdded.First().Name == message.From.Name) 
                {
                    var connector = new ConnectorClient(new Uri(message.ServiceUrl));
                    var welcomeMessage = message.CreateReply(
                        $"Hola {message.From.Name}.\r\nBienvenid@ al chat de la Asociación Española Contra el Cáncer.");
                    await connector.Conversations.ReplyToActivityAsync(welcomeMessage);
                    
                    // Not available in all channels
                    if (message.MembersRemoved != null && message.MembersRemoved.Count > 0)
                    {
                        foreach (ChannelAccount channelAccount in message.MembersRemoved)
                        {
                            Party party = new Party(
                                message.ServiceUrl, message.ChannelId, channelAccount, message.Conversation);

                            if (MessageRouterManager.RemoveParty(party).Any())
                            {
                                Debug.WriteLine($"Party {party.ToString()} removed");
                            }
                        }
                    }
                }
            }
            else if (message.Type == ActivityTypes.ContactRelationUpdate)
            {
                // Handle add/remove from contact lists
                // Activity.From + Activity.Action represent what happened
            }
            else if (message.Type == ActivityTypes.Typing)
            {
                // Handle knowing tha the user is typing
            }
            else if (message.Type == ActivityTypes.Ping)
            {
            }
        }
    }
}