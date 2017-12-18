using AeccApp.Core.Messages;
using AeccApp.Core.Services;
using AeccApp.Core.ViewModels.Popups;
using System;
using System.Collections.Generic;
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
            SettingsList.Add(this["SettingsPrivacity"]);
            SettingsList.Add(this["SettingsLastNews"]);
            SettingsList.Add(this["SettingsServicesInfo"]);
            SettingsList.Add(this["SettingsContactWithProfesional"]);       
        }

        public override Task ActivateAsync()
        {
            LogoutPopupVM.Logout += OnLogoutPopupLogout;
            MessagingCenter.Send(new ToolbarMessage(this["SettingsToolbarTitle"]), string.Empty);

            return Task.CompletedTask;
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

        private Command _chooseOptionCommand;
        public ICommand ChooseOptionCommand
        {
            get
            {
                return _chooseOptionCommand ??
                    (_chooseOptionCommand = new Command(OnChooseOptionCommand, o => !IsBusy));
            }
        }

        public async void OnChooseOptionCommand(object obj)
        {
            string i = obj as string;                  
            switch (i)
            {
                case "Privacidad de datos":
                    break;
                case "Últimas noticias":
                    await NavigationService.NavigateToAsync<AllNewsViewModel>();
                    break;
                case "Información sobre servicios":
                    break;
                case "Contactar con un profesional":
                    Device.OpenUri(new Uri("tel://900100036"));
                    break;
            }
         

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
        private List<String> _settingsList = new List<string>();

        public List<String> SettingsList
        {
            get { return _settingsList; }
            set { Set(ref _settingsList, value); }
        }



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
