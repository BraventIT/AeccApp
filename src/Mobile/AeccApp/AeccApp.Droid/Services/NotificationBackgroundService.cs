using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using AeccApp.Core.Services;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Xamarin.Forms;

namespace AeccApp.Droid.Services
{
    [Service(Name = "AeccApp.Droid.Services.NotificationBackgroundService")]
    public class NotificationBackgroundService : Service
    {
        public override IBinder OnBind(Intent intent)
        {
            throw new NotImplementedException();
        }

        [return: GeneratedEnum]
        public override StartCommandResult OnStartCommand(Intent intent, [GeneratedEnum] StartCommandFlags flags, int startId)
        {

            new Task(() =>
            {
                var t = new Thread(() =>
                {
                    while (true)
                    {

                        Thread.Sleep(10000);//Frequency on miliseconds to check for messages

                        //Check for new chat messages here:
                        /*
                        ServiceLocator.NotificationService.CreateNotification("Nuevo mensaje:", "Tienes nuevos mensajes, para a la app de Aecc para comprobarlos");
                        
                        */     
                    }
                });

                t.Start();

            }).Start();

            return StartCommandResult.Sticky;
        }

        public override void OnDestroy()
        {
            Intent broadcastIntent = new Intent(Android.App.Application.Context, typeof(NotificationServiceBroadcastReceiver));
            SendBroadcast(broadcastIntent);
        }

    }
}