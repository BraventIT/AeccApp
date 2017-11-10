using Aecc.Models;
using AeccApp.Core.Models;
using System.Collections.ObjectModel;

namespace AeccApp.Core
{
    public class GlobalSetting
    {
        private string _baseEndpoint;
        private string _apiEndpoint;
        private static readonly GlobalSetting _instance = new GlobalSetting();

        public GlobalSetting()
        {
            GooglePlacesApiKey = "AIzaSyBrlsD3dhg3Bo6oaAugOCexgVoNcQDaQgQ";

            // AZURE TEST BOT
            AeccBotSecret = "xb0KJwZiN3g.cwA.Crw.MkqbuqDBWCEtkOv8sprBqScs6AWw9IBDKaFXzjnh3M8";

            AzureAdB2COptions = new AzureAdB2COptions()
            {
                Tenant = "alfraso.onmicrosoft.com",
                ClientId = "24b548ab-933a-47c0-9b3c-c9b334219cc7",
                SignUpSignInPolicyId = "B2C_1_inicioDeSesion",
                EditProfilePolicyId = "B2C_1_perfilxamarin",
                ResetPasswordPolicyId = "B2C_1_resetPassword",
                AppID = "api",
                Scope = "read"
            };

            ApiEndpoint = "http://alfraso-aeccapi.azurewebsites.net";
        }

        public static GlobalSetting Instance
        {
            get { return _instance; }
        }

        public string AppID
        {
            get { return _baseEndpoint; }
            set { _baseEndpoint = value; }
        }

        public string ApiEndpoint
        {
            get { return _apiEndpoint; }
            set { _apiEndpoint = value; }
        }

        #region GlobalLists
        public ObservableCollection<Hospital> Hospitals { get; set; }
        #endregion

        #region Maps

        public string GooglePlacesApiKey { get; set; }

        #endregion

        #region Bot data
        public string AeccBotSecret { get; set; }
        #endregion

        #region Identity data
        public AzureAdB2COptions AzureAdB2COptions { get; set; }
        #endregion

        #region User data
        public UserData User { get; set; }
        #endregion
        
    }
}
