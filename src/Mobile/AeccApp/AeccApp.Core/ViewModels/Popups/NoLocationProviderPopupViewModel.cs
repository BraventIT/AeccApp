using AeccApp.Core.Services;
using System.Windows.Input;
using Xamarin.Forms;

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
                    (_continueToSettingsCommand = new Command
                    (o => LocationProviderSettings.OpenLocationProviderSettings()));
            }
        }
    }
}
