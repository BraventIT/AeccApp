using System;
using System.Windows.Input;
using Xamarin.Forms;

namespace AeccApp.Core.ViewModels.Popups
{
    public class RequestAskForAddressNumberPopupViewModel : ViewModelBase
    {
        public event EventHandler ContinueWithoutInputANumber;


        private Command _continueWithoutInputANumberCommand;
        public ICommand ContinueWithoutInputANumberCommand
        {
            get
            {
                return _continueWithoutInputANumberCommand ??
                    (_continueWithoutInputANumberCommand = new Command(o => ContinueWithoutInputANumber?.Invoke(this, null)));
            }
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
