using AeccApp.Core.IDependencyServices;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace AeccApp.Core.ViewModels
{
    public class NoLocationProviderPopupViewModel : ViewModelBase
    { 
        private Command _continueToSettingsCommand;
        public ICommand ContinueToSettingsCommand
        {
            get
            {
                return _continueToSettingsCommand ??
                    (_continueToSettingsCommand = new Command(OnContinueToSettingsCommand));
            }
        }
        private void OnContinueToSettingsCommand()
        {
            DependencyService.Get<ILocationProviderSettings>().OpenLocationProviderSettings();
        }

        private Command _closePopupCommand;
        public ICommand ClosePopupCommand
        {
            get
            {
                return _closePopupCommand ??
                    (_closePopupCommand = new Command(OnClosePopupCommand));
            }
        }
        private async void OnClosePopupCommand()
        {
            await NavigationService.HidePopupAsync();
            await NavigationService.NavigateToAsync<NewHospitalAddressViewModel>();
        }



    }
}
