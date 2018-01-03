using Aecc.Models;
using AeccApp.Core;
using AeccApp.Core.Services;
using Foundation;
using Microsoft.Bot.Connector.DirectLine;
using Microsoft.Identity.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UIKit;

namespace AeccApp.iOS
{
    [Register("AppDelegate")]
    public partial class AppDelegate : global::Xamarin.Forms.Platform.iOS.FormsApplicationDelegate
    {
        private const string GoogleIOSMapKey = "AIzaSyCYrC9dTd2tLpBuPEbFqmmJHyO-hJywvFk";


        private GlobalSetting GSetting { get { return GlobalSetting.Instance; } }
        private DirectLineClient _client = null;
        private Conversation _mainConversation;
        private ChannelAccountWithUserData _account;

        private IList<AeccApp.Core.Models.Message> _conversationMessages;
        public event EventHandler<IList<AeccApp.Core.Models.Message>> MessagesReceived;

        public override bool FinishedLaunching(UIApplication app, NSDictionary options)
        {
            global::Xamarin.Forms.Forms.Init();
            Xamarin.FormsGoogleMaps.Init(GoogleIOSMapKey);
            _conversationMessages = new List<AeccApp.Core.Models.Message>();
            _client = new DirectLineClient(GSetting.AeccBotSecret);
            LoadApplication(new App());

            if (options != null)
            {
                // Checks for local notifications received asleep 
                if (options.ContainsKey(UIApplication.LaunchOptionsLocalNotificationKey))
                {
                    var localNotification = options[UIApplication.LaunchOptionsLocalNotificationKey] as UILocalNotification;
                    if (localNotification != null)
                    {
                        UIAlertController okayAlertController = UIAlertController.Create(localNotification.AlertAction, localNotification.AlertBody, UIAlertControllerStyle.Alert);
                        okayAlertController.AddAction(UIAlertAction.Create("OK", UIAlertActionStyle.Default, null));
                        Window.RootViewController.PresentViewController(okayAlertController, true, null);
                        UIApplication.SharedApplication.ApplicationIconBadgeNumber = 0;
                    }
                }
            }

            //Ask for permission to use notifications
            if (UIDevice.CurrentDevice.CheckSystemVersion(8, 0))
            {
                var notificationSettings = UIUserNotificationSettings.GetSettingsForTypes(
                    UIUserNotificationType.Alert | UIUserNotificationType.Badge | UIUserNotificationType.Sound, null
                );

                app.RegisterUserNotificationSettings(notificationSettings);
            }
            return base.FinishedLaunching(app, options);
        }
        public override void DidEnterBackground(UIApplication application)
        {
            var taskID = UIApplication.SharedApplication.BeginBackgroundTask(() => { });
            new Task(() => {
                ListenToBotMessages();
            }).Start();
        }


        public override void ReceivedLocalNotification(UIApplication application, UILocalNotification notification)
        {
            // show an alert
            UIAlertController okayAlertController = UIAlertController.Create(notification.AlertAction, notification.AlertBody, UIAlertControllerStyle.Alert);
            okayAlertController.AddAction(UIAlertAction.Create("OK", UIAlertActionStyle.Default, null));
            var window = UIApplication.SharedApplication.KeyWindow;
            window.RootViewController.PresentViewController(okayAlertController, true, null);

            // reset our badge
            UIApplication.SharedApplication.ApplicationIconBadgeNumber = 0;
        }
        public override bool OpenUrl(UIApplication app, NSUrl url, NSDictionary options)
		{
			AuthenticationContinuationHelper.SetAuthenticationContinuationEventArgs(url);
			return true;
		}


        private async void ListenToBotMessages()
        {
            _mainConversation = await _client.Conversations.StartConversationAsync();
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

        public bool MessagesWitoutReading { get; private set; }


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
