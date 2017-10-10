using Microsoft.Bot.Builder.Dialogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Bot.Connector;
using AeccBot.MessageRouting;
using AeccBot.Models;
using AeccBot.MessageRouting.DataStore;

namespace AeccBot.Dialogs
{
    [Serializable]
    public class SelectAggregationDialog : IDialog<object>
    {
       
        private MessageRouterManager MessageRouterManager
        {
            get { return WebApiConfig.MessageRouterManager; }
        }

        private IRoutingDataManager RoutingDataManager
        {
            get { return WebApiConfig.RoutingDataManager; }
        }

        private IMessageRouterResultHandler MessageRouterResultHandler
        {
            get { return WebApiConfig.MessageRouterResultHandler; }
        }

        public Task StartAsync(IDialogContext context)
        {
            context.Wait(MessageReceivedAsync);
            return Task.CompletedTask;
        }

        public async virtual Task MessageReceivedAsync(IDialogContext context, IAwaitable<IMessageActivity> result)
        {
            var message = await result;

            var aggregations = RoutingDataManager.GetAggregationParties();

            if (aggregations.Any())
            {
                DisplayOptions(context, aggregations);
            }
            else
            {
                var noAggreationsMessage = context.MakeMessage();
                noAggreationsMessage.Text = AeccStrings.AddNoAgentsAvailable_Text;
                await context.PostAsync(noAggreationsMessage);
                noAggreationsMessage = context.MakeMessage();
                noAggreationsMessage.Text = AeccStrings.AddNoAgentsAvailable_Text2;
                await context.PostAsync(noAggreationsMessage);
            }
        }

        public void DisplayOptions(IDialogContext context, IList<Party> aggreagations)
        {
            List<string> optionKeys = new List<string>();
            List<string> optionValues = new List<string>();
            int cnt = 1;
            foreach (var party in aggreagations)
            {
                optionKeys.Add(cnt.ToString());
                optionValues.Add($"{cnt++} - {party.ChannelAccount.Name}");
            }

            PromptDialog.Choice(
                context,
                ProcessSelectedOptionAsync,
                optionKeys,
                "Por favor, ¿con qué número de voluntario quieres hablar?",
                "Ooops, has escrito una opción que no sé interpretar. Por favor, inténtalo otra vez",
                2,
                PromptStyle.PerLine,
                optionValues);
        }


        public async Task ProcessSelectedOptionAsync(IDialogContext context, IAwaitable<string> argument)
        {
            try
            {
                var selectedOption = await argument;
                var messageRouterResult = MessageRouterManager.RequestConnection(context.Activity as Activity, selectedOption);

                // Handle the result, if required
                await MessageRouterResultHandler.HandleResultAsync(messageRouterResult);
            }
            catch (Exception)
            {
            }
            context.Done(string.Empty);
        }
    }
}