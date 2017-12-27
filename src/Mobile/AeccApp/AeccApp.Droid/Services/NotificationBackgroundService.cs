using System;
using System.Threading;
using System.Threading.Tasks;
using AeccApp.Core.Services;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;

namespace AeccApp.Droid.Services
{
    [Service(Name = "aeccapp.droid.services.NotificationBackgroundService")]
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

                        Thread.Sleep(30000);//Frequency on miliseconds to check for messages
                        //Check for new chat messages here
                       // Este ejemplo descomentado genera una notificacion generica cada 30 segundos:
                       ServiceLocator.NotificationService.CreateNotification("Nuevo mensaje:", "Tienes nuevos mensajes, para a la app de Aecc para comprobarlos");
                        
                            
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