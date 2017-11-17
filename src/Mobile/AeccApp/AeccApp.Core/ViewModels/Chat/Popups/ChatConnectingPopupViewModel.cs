using System.Threading.Tasks;

namespace AeccApp.Core.ViewModels.Popups
{
    public class ChatConnectingPopupViewModel : ViewModelBase
    {
        public override Task ActivateAsync()
        {
            IsBusy = true;
            return Task.CompletedTask;
        }

        public override void Deactivate()
        {
            IsBusy = false;
        }
        public override bool OnBackButtonPressed()
        {
            return true;
        }
    }
}
