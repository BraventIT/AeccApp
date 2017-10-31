using System;
using System.Windows.Input;
using Xamarin.Forms;

namespace AeccApp.Core.ViewModels.Popups
{
    public class LogoutPopupViewModel : ClosablePopupViewModelBase
    {
        /// <summary>
        /// Raised when filters are applied
        /// </summary>
        public event EventHandler Logout;

        private Command _logoutCommand;
        public ICommand LogoutCommand
        {
            get
            {
                return _logoutCommand ??
                    (_logoutCommand = new Command(o => Logout?.Invoke(this, null)));
            }
        }
    }
}
