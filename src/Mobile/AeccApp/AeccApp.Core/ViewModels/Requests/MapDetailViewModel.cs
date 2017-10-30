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
            Position = (Position)navigationData;

            return Task.CompletedTask;
        }

        public override Task ActivateAsync()
        {
            MessagingCenter.Send(new GeolocatorMessages(GeolocatorEnum.Refresh), string.Empty, Position);

            return base.ActivateAsync();
        }



        #region Properties
        private Position _position;

        public Position Position
        {
            get { return _position; }
            set { Set(ref _position, value); }
        }

        #endregion


    }
}
