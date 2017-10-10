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
        private readonly IIdentityService _identityService;

        #region Activate & Deactive Methods
        public LoginViewModel()
        {
            _identityService = ServiceLocator.Resolve<IIdentityService>();
        }

        public override Task ActivateAsync()
        {
            return TryToLoginAsync(true);
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
                try
                {
                    //_identityService.LogOff();
                    if (await _identityService.TryToLoginAsync(silentLogin))
                    {
                        await NavigationService.NavigateToAsync<VolunteerTestViewModel>();
                        await NavigationService.RemoveLastFromBackStackAsync();
                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.Message);
                }
            });
        }

       

       

        protected override void OnIsBusyChanged()
        {
            _signInCommand.ChangeCanExecute();
        }
#endregion
    }
}
