using AeccApp.Core.Services;
using AeccApp.Core.ViewModels;
using Xamarin.Forms;
using Microsoft.Identity.Client;
using AeccApp.Core;

namespace AeccApp
{
    public partial class App : Application
    {
        public static UIParent UiParent = null;

        public App()
        {
            InitializeComponent();
            InitApp();

            if (Device.RuntimePlatform == Device.UWP)
            {
                InitNavigation();
            }
        }

        private void InitApp()
        {
            ServiceLocator.RegisterDependencies();
            ViewModelLocator.RegisterDependencies();
            IocContainer.Build();
        }

        private void InitNavigation()
        {
            var navigationService = ServiceLocator.NavigationService;
            navigationService.Initialize();
        }

        protected override void OnStart()
        {
            if (Device.RuntimePlatform != Device.UWP)
            {
                InitNavigation();
            }
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}
