using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace AeccApp.Core.ViewModels.Popups
{
    public class RetryPopupViewModel: ViewModelBase
    {
        Func<Task> _retryAction;

        public RetryPopupViewModel(Func<Task> retryAction)
        {
            _retryAction = retryAction;
        }

        private Command _retryCommand;
        public ICommand RetryCommand
        {
            get
            {
                return _retryCommand ??
                    (_retryCommand = new Command(async o => await OnRetryAsync()));
            }
        }

        private async Task OnRetryAsync()
        {
            await NavigationService.HidePopupAsync();
            await _retryAction.Invoke();
        }

        public override bool OnBackButtonPressed()
        {
            return true;
        }
    }
}
