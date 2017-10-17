using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace AeccApp.Core.ViewModels.Popups
{
   public class RequestSentPopupViewModel : ViewModelBase
    {
        
 private Command _closeRequestSentPopupCommand;
        public ICommand CloseRequestSentPopupCommand
        {
            get
            {
                return _closeRequestSentPopupCommand ??
                    (_closeRequestSentPopupCommand = new Command(OnClosePopupCommand));
            }
        }
        private async void OnClosePopupCommand()
        {
            await NavigationService.HidePopupAsync();
            await NavigationService.NavigateToAsync<DashboardViewModel>();
        }

    }
}
