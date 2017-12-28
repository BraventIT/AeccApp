using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Aecc.Models;
using AeccApp.Core;
using AeccApp.Core.Services;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Microsoft.Bot.Connector.DirectLine;

namespace AeccApp.Droid.Services
{
    [Service(Name = "aeccapp.droid.services.NotificationBackgroundService")]
    public class NotificationBackgroundService : Service
    {

        private GlobalSetting GSetting { get { return GlobalSetting.Instance; } }
        private DirectLineClient _client = null;
        private Conversation _mainConversation;
        private ChannelAccountWithUserData _account;

        private IList<AeccApp.Core.Models.Message> _conversationMessages;
        public event EventHandler<IList<AeccApp.Core.Models.Message>> MessagesReceived;


        public bool MessagesWitoutReading { get; private set; }


        public override IBinder OnBind(Intent intent)
        {
            throw new NotImplementedException();
        }

        public override void OnTaskRemoved(Intent rootIntent)
        {
            StopSelf();
            base.OnTaskRemoved(rootIntent);
        }

        public override void OnDestroy()
        {
            Intent broadcastIntent = new Intent(Android.App.Application.Context, typeof(NotificationServiceBroadcastReceiver));
            SendBroadcast(broadcastIntent);
            base.OnDestroy();
        }

        public async void InitializeAsync()
        {
            _mainConversation = await _client.Conversations.StartConversationAsync();
        }

        [return: GeneratedEnum]
        public override StartCommandResult OnStartCommand(Intent intent, [GeneratedEnum] StartCommandFlags flags, int startId)
        {
            _conversationMessages = new List<AeccApp.Core.Models.Message>();
            _client = new DirectLineClient(GSetting.AeccBotSecret);
            if (_mainConversation == null)
                InitializeAsync();


            var user = GSetting.User;
            _account = new ChannelAccountWithUserData()
            {
                Id = user.Id,
                Name = user.Name,
                FirstName = user.FirstName,
                Surname = user.Surname,
                Email = user.Email,
                Age = user.Age,
                Gender = user.Gender
            };


            new Task(() =>
            {
                var t = new Thread(() =>
                {
                    while (true)
                    {
                        //Frequency on miliseconds to check for messages
                        Thread.Sleep(5000);
                        //Check for new chat messages here
                        ListenToBotMessages();


                    }
                });

                t.Start();

            }).Start();

            return StartCommandResult.Sticky;
        }
      

    



        private async void ListenToBotMessages()
        {
            string watermark = null;

            while (true)
            {
                try
                {
                    var activitySet = await _client.Conversations.GetActivitiesAsync(_mainConversation.ConversationId, watermark);
                    // only new messages required
                    watermark = activitySet?.Watermark;

                    List<AeccApp.Core.Models.Message> newMessages = new List<AeccApp.Core.Models.Message>();
                    foreach (var activity in activitySet.Activities)
                    {
                        // oneselft messages discarded
                        if (activity.From.Id != _account.Id)
                        {
                            switch (activity.Type)
                            {
                                case ActivityTypes.Message:
                                    newMessages.Add(ProcessMessage(activity)); break;
                                default:
                                    break;
                            }
                        }
                    }

                    if (newMessages.Any())
                    {
                        foreach (var message in newMessages)
                        {
                            _conversationMessages.Insert(0, message);
                        }


                        MessagesReceived?.Invoke(this, newMessages);
                        MessagesWitoutReading = MessagesReceived == null;
                        if (MessagesWitoutReading)
                        {
                            ServiceLocator.NotificationService.CreateNotification("Nuevo mensaje:", newMessages[newMessages.Count - 1].Activity.Text);
                        }
                    }
                }
                catch (Exception ex)
                {

                }
               
            }
        }

         private AeccApp.Core.Models.Message ProcessMessage(Microsoft.Bot.Connector.DirectLine.Activity activity)
        {
            // Removing extra * we don't want to see in the output
            activity.Text = activity.Text.Replace("\r\n*", "\r\n");

            var message = new AeccApp.Core.Models.Message
            {
                DateTime = activity.Timestamp.Value,
                Activity = activity
            };

            return message;
        }


    }
}