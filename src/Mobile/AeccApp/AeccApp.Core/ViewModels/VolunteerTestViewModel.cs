using AeccApp.Core.Services;
using System.Windows.Input;
using Xamarin.Forms;
using System.Threading.Tasks;
using AeccApp.Core.ViewModels.Popups;
using AeccApp.Core.Messages;

namespace AeccApp.Core.ViewModels
{
    public class VolunteerTestViewModel : ViewModelBase
    {
        private readonly IIdentityService _identityService;

        public VolunteerTestViewModel()
        {
            _identityService = ServiceLocator.IdentityService;
        }

        public async override Task ActivateAsync()
        {
            // var kk = await ServiceLocator.HomeRequestService.GetRequestTypesAsync();
            //  var kk2 = await ServiceLocator.HomeRequestService.GetCoordinatorsAsync("zaragoza");

            //test POPUPS
            //await NavigationService.ShowPopupAsync(new ChatEventPopupViewModel(), new ChatEventMessage( AeccBot.MessageRouting.MessageRouterResultType.ConnectionRejected, "test"));
        }

        private Command _enterCommand;
        public ICommand EnterCommand
        {
            get
            {
                return _enterCommand ??
                    (_enterCommand = new Command(
                        async o => await NavigationService.NavigateToAsync<DashboardViewModel>()));
            }
        }
    }
}
