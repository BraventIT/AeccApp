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
        private async void OnContinueToSettingsCommand()
        {

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
        }



    }
}
