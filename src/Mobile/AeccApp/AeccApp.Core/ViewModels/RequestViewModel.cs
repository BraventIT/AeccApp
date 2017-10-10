using AeccApp.Internationalization.Properties;
using System.Resources;
using System.Windows.Input;
using Xamarin.Forms;

namespace AeccApp.Core.ViewModels
{
    public class RequestsViewModel : ViewModelBase
    { 

        #region Commands
        private Command _newRequestCommand;
        public ICommand NewRequestCommand
        {
            get
            {
                return _newRequestCommand ??
                    (_newRequestCommand = new Command(OnNewRequestCommand, (o) => !IsBusy));
            }
        }
        /// <summary>
        /// Navigates to NewRequestSelectAddressView
        /// </summary>
        /// <param name="obj"></param>
         async void OnNewRequestCommand(object obj)
        {
            await NavigationService.NavigateToAsync<NewRequestSelectAddressViewModel>();
        }



        #endregion
    }
}
