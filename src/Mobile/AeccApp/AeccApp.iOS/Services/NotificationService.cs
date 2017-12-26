using AeccApp.Core.Services;
using AeccApp.iOS.Services;
using Foundation;
using UIKit;

[assembly: Xamarin.Forms.Dependency(typeof(NotificationService))]
namespace AeccApp.iOS.Services
{

    class NotificationService : INotificationService
    {
        public void CreateNotification(string title, string body)
        {
            // create the notification
            var notification = new UILocalNotification();

            // set the fire date (the date time in which it will fire)
            notification.FireDate = NSDate.FromTimeIntervalSinceNow(1);

            // configure the alert
            notification.AlertAction = title;
            notification.AlertBody = body;

            // modify the badge
            notification.ApplicationIconBadgeNumber = 1;

            // set the sound to be the default sound
            notification.SoundName = UILocalNotification.DefaultSoundName;

            // schedule it
            UIApplication.SharedApplication.ScheduleLocalNotification(notification);
        }
    }
}