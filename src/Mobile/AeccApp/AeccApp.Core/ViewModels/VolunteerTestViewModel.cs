using AeccApp.Core.Services;
using System.Windows.Input;
using Xamarin.Forms;

namespace AeccApp.Core.ViewModels
{
    public class VolunteerTestViewModel : ViewModelBase
    {
        private readonly IIdentityService _identityService;

        public VolunteerTestViewModel()
        {
            _identityService = ServiceLocator.Resolve<IIdentityService>();
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
