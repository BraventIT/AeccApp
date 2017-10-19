using AeccApp.Core.Services;
using System;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using System.Diagnostics;

namespace AeccApp.Core.ViewModels
{
    public class LoginViewModel : ViewModelBase
    {
        private IIdentityService IdentityService { get; } = ServiceLocator.IdentityService;

        #region Activate & Deactive Methods
        public override Task ActivateAsync()
        {
            return TryToLoginAsync(true);
        }
        #endregion

        #region Properties
        private bool _isLoginRequired;
        public bool IsLoginRequired
        {
            get { return _isLoginRequired; }
            set { Set(ref _isLoginRequired, value); }
        }
        #endregion

        #region Commands
        private Command _signInCommand;
        public ICommand SignInCommand
        {
            get
            {
                return _signInCommand ??
                    (_signInCommand = new Command(
                        o => TryToLoginAsync(false),
                        o => !IsBusy));
            }
        }

        #endregion

        #region Private Methods
        private Task TryToLoginAsync(bool silentLogin)
        {
            return ExecuteOperationAsync(async () =>
            {
                if (await IdentityService.TryToLoginAsync(silentLogin))
                {
                    await NavigationService.NavigateToAsync<VolunteerTestViewModel>();
                    await NavigationService.RemoveLastFromBackStackAsync();
                }
            }, finallyAction: () => IsLoginRequired = true);
        }

        protected override void OnIsBusyChanged()
        {
            _signInCommand.ChangeCanExecute();
        }
#endregion
    }
}
