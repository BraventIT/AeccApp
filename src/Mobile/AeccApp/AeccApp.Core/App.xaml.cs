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

        public static PublicClientApplication PCA = null;

        // Azure AD B2C Coordinates
        public static string Tenant = "alfraso.onmicrosoft.com";
        public static string ClientID = "24b548ab-933a-47c0-9b3c-c9b334219cc7";
        public static string PolicySignUpSignIn = "B2C_1_inicioDeSesion";
        public static string PolicyEditProfile = "B2C_1_perfilxamarin";
        public static string PolicyResetPassword = "B2C_1_passxamarin";
        public static string ApiEndpoint = $"https://{Tenant}/demoapi";

        public static string[] Scopes = { $"{ApiEndpoint}/demo.read", $"{ApiEndpoint}/user_impersonation" };

        public static string AuthorityBase = $"https://login.microsoftonline.com/tfp/{Tenant}/";
        public static string Authority = $"{AuthorityBase}{PolicySignUpSignIn}";
        public static string AuthorityEditProfile = $"{AuthorityBase}{PolicyEditProfile}";
        public static string AuthorityPasswordReset = $"{AuthorityBase}{PolicyResetPassword}";

        public App()
        {
            InitializeComponent();

            // default redirectURI; each platform specific project will have to override it with its own
            PCA = new PublicClientApplication(ClientID, Authority);
            PCA.RedirectUri = $"msal{ClientID}://auth";

            InitApp();

            if (Device.RuntimePlatform == Device.Windows)
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
            var navigationService = ServiceLocator.Resolve<INavigationService>();
            navigationService.Initialize();
        }

        protected override void OnStart()
        {
            if (Device.RuntimePlatform != Device.Windows)
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
