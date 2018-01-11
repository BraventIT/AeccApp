using Aecc.Models;
using AeccApp.Core.Models;
using System.Collections.ObjectModel;

namespace AeccApp.Core
{
    public class GlobalSetting
    {
        public const string TEST_EMAIL = "afraj@bravent.net";

        private string _baseEndpoint;
        private string _apiEndpoint;
        private static readonly GlobalSetting _instance = new GlobalSetting();

        public GlobalSetting()
        {
            GooglePlacesApiKey = "AIzaSyBrlsD3dhg3Bo6oaAugOCexgVoNcQDaQgQ";

            // AZURE TEST BOT
            AeccBotSecret = "3EFssRtgdBI.cwA.IF8.a-Slt6ur7daidGUxQZZdfS-HiLsWVeNQOcWv8mdLubM";
            // LOCAL TEST BOT ALFRA
            //AeccBotSecret = "Ch31g-A6LJo.cwA.AZo.-GJpdQLh0kGznJxIlBn926-dKWSrW3qXTC81cNRF6wc";

            AzureAdB2COptions = new AzureAdB2COptions()
            {
                Tenant = "alfraso.onmicrosoft.com",
                ClientId = "24b548ab-933a-47c0-9b3c-c9b334219cc7",
                SignUpSignInPolicyId = "B2C_1_SignUpSignIn",
                EditProfilePolicyId = "B2C_1_EditProfile",
                ResetPasswordPolicyId = "B2C_1_resetPassword",
                AppID = "aeccapi",
                Scope = "read"
            };

            // AZURE TEST API
            ApiEndpoint = "http://alfraso-aeccapi.azurewebsites.net";
            // LOCAL TEST API ALFRA
            //ApiEndpoint = "http://localhost:14724";

            EmailChatAddress = TEST_EMAIL;
            EmailChatTemplate =
                "El beneficiario %UserName% %UserSurname% ha reportado una conversación con " +
                "%CounterpartName% %CounterpartSurname% cuya valoración es %ChatRating%.\n" +
                "Detalle de conversación:\n\n%Conversation%";
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
        
        public CurrentUser User { get; set; }
        
        #endregion

        #region Chat

        public string EmailChatTemplate { get; set; }
        public string EmailChatAddress { get; set; }

        #endregion

    }
}
