using System.Threading.Tasks;
using AeccApp.Core.Services;
using Xamarin.Forms;
using System.Windows.Input;
using AeccApp.Core.ViewModels.Popups;
using System;

namespace AeccApp.Core.ViewModels
{
    public class ProfileViewModel : ViewModelBase
    {
        private readonly IIdentityService _identityService;

        public ProfileViewModel()
        {
            _identityService = ServiceLocator.IdentityService;
            LogoutPopupVM = new LogoutPopupViewModel();
        }

        public override async Task ActivateAsync()
        {
            LogoutPopupVM.Logout += OnLogoutPopupLogout;
        }

        public override void Deactivate()
        {
            LogoutPopupVM.Logout -= OnLogoutPopupLogout;
        }

        #region Properties
        public string Name
        {
            get { return GSetting.User?.Name; }
        }

        public string Email
        {
            get { return GSetting.User?.Email; }
        }

        public string Telephone
        {
            get { return string.Empty; }
        }

        public string Address
        {
            get { return GSetting.User?.Address; }
        }

        public int Age
        {
            get { return GSetting.User.Age; }
        }

        public LogoutPopupViewModel LogoutPopupVM { get; private set; }

        #endregion

        #region Commands
        /// <summary>
        /// Show logout popup
        /// </summary>
        private Command _showLogoutPopupCommand;
        public ICommand ShowLogoutPopupCommand
        {
            get
            {
                return _showLogoutPopupCommand ??
                    (_showLogoutPopupCommand = new Command(o => NavigationService.ShowPopupAsync(LogoutPopupVM)));
            }
        }
         
        private Command _editProfileCommand;
        public ICommand EditProfileCommand
        {
            get
            {
                return _editProfileCommand ??
                    (_editProfileCommand = new Command(o => OnEditProfileAsync()));
            }
        }
        /// <summary>
        /// Opens webview to edit profile
        /// </summary>
        /// <returns></returns>
        private async Task OnEditProfileAsync()
        {
           await _identityService.EditProfileAsync();
            NotifyPropertyChanged(nameof(Name));
            NotifyPropertyChanged(nameof(Email));
        }

        #endregion

        /// <summary>
        /// Logout 
        /// </summary>
        /// <returns></returns>
        private async void OnLogoutPopupLogout(object sender, EventArgs e)
        {
            await NavigationService.HidePopupAsync();
            _identityService.LogOff();
            await NavigationService.NavigateToAsync<LoginViewModel>();
            await NavigationService.RemoveBackStackAsync();
        }
    }
}
