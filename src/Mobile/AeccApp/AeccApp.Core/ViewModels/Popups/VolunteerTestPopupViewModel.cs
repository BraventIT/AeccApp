using System;
using System.Windows.Input;
using Xamarin.Forms;

namespace AeccApp.Core.ViewModels.Popups
{
    public class VolunteerTestPopupViewModel : ViewModelBase
    {
        public event EventHandler Continue;

        private Command _enterCommand;
        public ICommand EnterCommand
        {
            get
            {
                return _enterCommand ??
                    (_enterCommand = new Command(o => Continue?.Invoke(this, null)));
            }
        }

        public override bool OnBackButtonPressed()
        {
            return true;
        }
    }
}
