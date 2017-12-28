using AeccApp.Core.Services;
using AeccApp.Droid.Services;
using Android.Content;

[assembly: Xamarin.Forms.Dependency(typeof(ChatMessagesListenerInit))]
namespace AeccApp.Droid.Services
{
    class ChatMessagesListenerInit : IChatMessagesListenerService
    {
            Intent notificationService;
        public void InitChatMessageListener()
        {         
            notificationService = new Intent(Android.App.Application.Context, typeof(NotificationBackgroundService));
            Android.App.Application.Context.StartService(notificationService);
        }

        
    }
}