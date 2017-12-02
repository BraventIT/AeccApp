using AeccApp.Core.Services;
using AeccApp.Core.ViewModels.Popups;
using Microsoft.Identity.Client;
using System;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace AeccApp.Core.ViewModels
{
    public class LoginViewModel : ViewModelBase
    {
        private IChatService ChatService { get; } = ServiceLocator.ChatService;
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

        private string _text;
        public string Text
        {
            get { return _text; }
            set { Set(ref _text, value); }
        }

        VolunteerTestPopupViewModel _volunteerTestPopupVM;
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
                    IsLoginRequired = false;
                    Text = (silentLogin) ?
                        this["LoginViewPreLoginSilentText"] :
                        this["LoginViewPreLoginText"];
                    if (await IdentityService.TryToLoginAsync(silentLogin))
                    {
                        #region ONLY FOR TEST
                        _volunteerTestPopupVM = new VolunteerTestPopupViewModel();
                        _volunteerTestPopupVM.Continue += OnVolunteerTestPopupVMContinue;
                        await NavigationService.ShowPopupAsync(_volunteerTestPopupVM);
                        #endregion

                        //Text = this["LoginViewPostLoginText"];
                        //await ContinueLoadingAsync();
                    }
                }
                catch (MsalException)
                {
                    //ex.ErrorCode== "request_timeout"
                    throw;
                }
            }, finallyAction: () => IsLoginRequired = true);
        }

        #region ONLY FOR TEST
        private async void OnVolunteerTestPopupVMContinue(object sender, EventArgs e)
        {
            IsLoginRequired = false;
            _volunteerTestPopupVM.Continue -= OnVolunteerTestPopupVMContinue;
            await NavigationService.HidePopupAsync();
            Text = this["LoginViewPostLoginText"];
            await ExecuteOperationAsync(async () => await ContinueLoadingAsync());
        }
        #endregion

        private async Task ContinueLoadingAsync()
        {
            await ChatService.InitializeAsync();
            await NavigationService.NavigateToAsync<DashboardViewModel>();
            //await NavigationService.RemoveLastFromBackStackAsync();
        }

        protected override void OnIsBusyChanged()
        {
            _signInCommand.ChangeCanExecute();
        }
        #endregion
    }
}
