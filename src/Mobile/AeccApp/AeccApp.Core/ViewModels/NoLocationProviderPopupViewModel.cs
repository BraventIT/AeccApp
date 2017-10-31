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

        public event EventHandler ClosePopup;


        private Command _closePopupCommand;
        public ICommand ClosePopupCommand
        {
            get
            {
                return _closePopupCommand ??
                    (_closePopupCommand = new Command(o => ClosePopup?.Invoke(this, null)));
            }
        }


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
    }
}
