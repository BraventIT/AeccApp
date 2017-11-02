using AeccApp.Core.Messages;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.GoogleMaps;

namespace AeccApp.Core.ViewModels
{
    public class MapDetailViewModel : ViewModelBase
    {
        private Position _position;

        public override Task InitializeAsync(object navigationData)
        {
            _position = (Position)navigationData;

            return Task.CompletedTask;
        }

        public override Task ActivateAsync()
        {
            MessagingCenter.Send(new GeolocatorMessage(_position), string.Empty);

            return Task.CompletedTask; 
        }
    }
}
