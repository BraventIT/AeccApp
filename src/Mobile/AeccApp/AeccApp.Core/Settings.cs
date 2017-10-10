// Helpers/Settings.cs
using Plugin.Settings;
using Plugin.Settings.Abstractions;

namespace AeccApp.Core
{
	/// <summary>
	/// This is the Settings static class that can be used in your Core solution or in any
	/// of your client applications. All settings are laid out the same exact way with getters
	/// and setters. 
	/// </summary>
	public static class Settings
	{
		private static ISettings AppSettings
        {
            get { return CrossSettings.Current; }
        }

		#region Setting Constants

		private const string AccessTokenKey = "access_token";
        private static readonly string AccessTokenDefault = string.Empty;

        private const string TermsAndConditionsAcceptKey = "TermsAndConditions_accept";
        private static readonly bool TermsAndConditionsAcceptDefault = false;

        #endregion


        public static string AuthAccessToken
        {
			get
			{
				return AppSettings.GetValueOrDefault(AccessTokenKey, AccessTokenDefault);
			}
			set
			{
				AppSettings.AddOrUpdateValue(AccessTokenKey, value);
			}
		}


        /// <summary>
        /// returns bool with TermsAndConditions current status
        /// </summary>
        public static bool TermsAndConditionsAccept
        {
            get
            {
                return AppSettings.GetValueOrDefault(TermsAndConditionsAcceptKey, TermsAndConditionsAcceptDefault);
            }
            set
            {
                AppSettings.AddOrUpdateValue(TermsAndConditionsAcceptKey, value);
            }
        }

    }
}