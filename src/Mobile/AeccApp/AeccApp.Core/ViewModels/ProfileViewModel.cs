using System.Threading.Tasks;
using AeccApp.Core.Services;
using Xamarin.Forms;
using System.Windows.Input;

namespace AeccApp.Core.ViewModels
{
    public class ProfileViewModel : ViewModelBase
    {
        private readonly IIdentityService _identityService;

        public ProfileViewModel()
        {
            _identityService = ServiceLocator.Resolve<IIdentityService>();
        }

        public override void Deactivate()
        {
            base.Deactivate();
            IsLogOutPopUpVisible = false;
        }

        #region Properties
        public string Name
        {
            get { return GSetting.User?.UserName; }
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

        private bool _isLogOutPopUpVisible;

        public bool IsLogOutPopUpVisible
        {
            get { return _isLogOutPopUpVisible; }
            set
            {
                Set(ref _isLogOutPopUpVisible, value);
            }
        }
        


        #endregion

        #region Commands
        private Command _logoutCommand;
        public ICommand LogoutCommand
        {
            get
            {
                return _logoutCommand ??
                    (_logoutCommand = new Command(o => OnLogoutAsync()));
            }
        }
        /// <summary>
        /// Logout 
        /// </summary>
        /// <returns></returns>
        private async Task OnLogoutAsync()
        {
            _identityService.LogOff();
            await NavigationService.NavigateToAsync<LoginViewModel>();
            await NavigationService.RemoveBackStackAsync();
        }

        private Command _openCloseLogoutPopupCommand;
        public ICommand OpenCloseLogoutPopupCommand
        {
            get
            {
                return _openCloseLogoutPopupCommand ??
                    (_openCloseLogoutPopupCommand = new Command(o => OnOpenCloseLogoutPopupCommand()));
            }
        }
        /// <summary>
        /// Opens and closes logout popup
        /// </summary>
        private void OnOpenCloseLogoutPopupCommand()
        {
            IsLogOutPopUpVisible = !IsLogOutPopUpVisible;
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


        public override bool OnBackButtonPressed()
        {
            bool returnValue = false;
            if (IsLogOutPopUpVisible)
            {
                IsLogOutPopUpVisible = false;
                returnValue = true;
            }

            return returnValue;
        }
    }
}
