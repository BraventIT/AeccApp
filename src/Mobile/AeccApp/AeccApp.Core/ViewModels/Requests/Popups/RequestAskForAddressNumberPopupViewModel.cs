using System;
using System.Windows.Input;
using Xamarin.Forms;

namespace AeccApp.Core.ViewModels.Popups
{
    public class RequestAskForAddressNumberPopupViewModel : ClosablePopupViewModelBase
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
    }
}
