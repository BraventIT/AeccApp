using AeccApp.Core.Services;
using AeccApp.Core.ViewModels.Popups;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace AeccApp.Core.ViewModels
{
   public class SettingsDashboardViewModel : ViewModelBase
    {
        private IIdentityService IdentityService { get; } = ServiceLocator.IdentityService;
        private IChatService ChatService { get; } = ServiceLocator.ChatService;


        #region Ctor and initialize
        public SettingsDashboardViewModel()
        {
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

        #endregion

        #region Commands
        private Command _showLogoutPopupCommand;
        public ICommand ShowLogoutPopupCommand
        {
            get
            {
                return _showLogoutPopupCommand ??
                    (_showLogoutPopupCommand = new Command(o => NavigationService.ShowPopupAsync(LogoutPopupVM)));
            }
        }


        private Command _talkToAeccCommand;
        public ICommand TalkToAeccCommand
        {
            get
            {
                return _talkToAeccCommand ??
                    (_talkToAeccCommand = new Command(OnTalkToAeccCommand, o => !IsBusy));
            }
        }

        public void OnTalkToAeccCommand(object obj)
        {
            //Llamada al telefono de infocancer de AECC
            Device.OpenUri(new Uri("tel://900100036"));

        }


        private Command openAllNewsCommand;
        public ICommand OpenAllNewsCommand
        {
            get
            {
                return openAllNewsCommand ??
                    (openAllNewsCommand = new Command(o => OnOpenAllNewsViewAsync()));
            }
        }

       
        private async Task OnOpenAllNewsViewAsync()
        {
            await NavigationService.NavigateToAsync<AllNewsViewModel>();
        }


        private Command _openUserProfile;
        public ICommand OpenUserProfile
        {
            get
            {
                return _openUserProfile ??
                    (_openUserProfile = new Command(OnOpenUserProfile));
            }
        }

        async void OnOpenUserProfile()
        {
            await NavigationService.NavigateToAsync<ProfileViewModel>();
        }
        #endregion


        #region Properties
        public LogoutPopupViewModel LogoutPopupVM { get; private set; }


        public string UserName
        {
            get { return GSetting.User?.Name; }
        }


        #endregion




        private async void OnLogoutPopupLogout(object sender, EventArgs e)
        {
            await NavigationService.HidePopupAsync();
            IdentityService.LogOff();
            await ChatService.LogOffAsync();
            await NavigationService.NavigateToAsync<LoginViewModel>();
            await NavigationService.RemoveBackStackAsync();
        }

    }
}
