using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace AeccApp.Core.ViewModels.Popups
{
    public class LogoutPopupViewModel : ViewModelBase
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

        /// <summary>
        /// Closes logout popup
        /// </summary>
        private Command _closePopupCommand;
        public ICommand ClosePopupCommand
        {
            get
            {
                return _closePopupCommand ??
                    (_closePopupCommand = new Command(o => NavigationService.HidePopupAsync()));
            }
        }
    }
}
