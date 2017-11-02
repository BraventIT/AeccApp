using AeccApp.Core.Services;
using System.Windows.Input;
using Xamarin.Forms;
using System.Threading.Tasks;

namespace AeccApp.Core.ViewModels.Popups
{
    public class NoLocationProviderPopupViewModel : ClosablePopupViewModelBase
    {
        private ILocationProviderSettings LocationProviderSettings { get; } = ServiceLocator.LocationProviderSettings;

        private Command _continueToSettingsCommand;
        public ICommand ContinueToSettingsCommand
        {
            get
            {
                return _continueToSettingsCommand ??
                    (_continueToSettingsCommand = new Command(o => OnContinueToSettingsAsync()));
            }
        }

        private async Task OnContinueToSettingsAsync()
        {
            LocationProviderSettings.OpenLocationProviderSettings();
            await NavigationService.HidePopupAsync();
        }
    }
}
