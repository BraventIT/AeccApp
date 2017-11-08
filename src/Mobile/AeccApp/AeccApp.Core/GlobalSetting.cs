using AeccApp.Core.Models;

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

            DomainName = "alfraso.onmicrosoft.com";
            ApplicationID = "24b548ab-933a-47c0-9b3c-c9b334219cc7";

            BaseEndpoint = $"https://{DomainName}/aeccapi";
            ApiEndpoint = "https://aecc.alfraso.com";

        }

        public static GlobalSetting Instance
        {
            get { return _instance; }
        }

        public string BaseEndpoint
        {
            get { return _baseEndpoint; }
            set { _baseEndpoint = value; }
        }

        public string ApiEndpoint
        {
            get { return _apiEndpoint; }
            set { _apiEndpoint = value; }
        }

        #region Maps

        public string GooglePlacesApiKey { get; set; }

        #endregion

        #region Bot data
        public string AeccBotSecret { get; set; }
        #endregion

        #region Identity data
        public string DomainName { get; set; }
        public string ApplicationID { get; set; }
        #endregion

        #region User data
        public UserData User { get; set; }
        #endregion
        
    }
}
