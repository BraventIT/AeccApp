using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AeccApp.Core.Services.Notification;
using Foundation;
using UIKit;

[assembly: Xamarin.Forms.Dependency(typeof(AeccApp.iOS.Notification))]
namespace AeccApp.iOS
{

    class Notification : INotification
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