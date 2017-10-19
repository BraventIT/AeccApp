using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace AeccApp.Core.ViewModels.Popups
{
   public class RequestThereIsNoResultsPopupViewModel : ViewModelBase
    {

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
