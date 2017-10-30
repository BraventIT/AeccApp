using AeccApp.Core.Messages;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.GoogleMaps;

namespace AeccApp.Core.ViewModels
{
    public class MapDetailViewModel : ViewModelBase
    {

        public override Task InitializeAsync(object navigationData)
        {
            var position = (Position)navigationData;
            MessagingCenter.Send(new GeolocatorMessages(GeolocatorEnum.Refresh), string.Empty, position);

            return Task.CompletedTask;
        }


        }
}
