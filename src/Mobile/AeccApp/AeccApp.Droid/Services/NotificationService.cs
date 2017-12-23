using AeccApp.Core.Services;
using AeccApp.Droid.Services;
using Android.App;
using Android.Content;
using Android.Content.Res;
using Android.Graphics;
using Android.Media;
using Android.Net;
using Android.Support.V4.App;

[assembly: Xamarin.Forms.Dependency(typeof(NotificationService))]
namespace AeccApp.Droid.Services
{

    class NotificationService : INotificationService
    {
        public void CreateNotification(string title, string body)
        {
            Uri sound = RingtoneManager.GetActualDefaultRingtoneUri(Application.Context,RingtoneType.Notification);
            int messageId = 999;
            NotificationCompat.Builder builder = new NotificationCompat.Builder(Application.Context)
                .SetAutoCancel(true)
                .SetContentTitle(title)
                .SetLargeIcon(BitmapFactory.DecodeResource(Resources.System,Resource.Drawable.icon))
                .SetSmallIcon(Resource.Drawable.icon)
                .SetContentText(body)
                .SetSound(sound)
                .SetStyle(new NotificationCompat.BigTextStyle().BigText(body))
                ;

            NotificationManager notificationManager = (NotificationManager)Application.Context.GetSystemService(Context.NotificationService);
            notificationManager.Notify(messageId, builder.Build());


        }
    }
}