﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace AeccApp.Droid.Services
{
    [BroadcastReceiver(Name = "aeccapp.droid.services.NotificationServiceBroadcastReceiver",Enabled =true,Exported =true)]
    [IntentFilter(new string[] { Intent.ActionBootCompleted , "RestartNotificationService" })]
    public class NotificationServiceBroadcastReceiver : BroadcastReceiver
    {
        public override void OnReceive(Context context, Intent intent)
        {
            Intent notificationService = new Intent(context, typeof(NotificationBackgroundService));
            context.StartService(notificationService);
        }
    }
}