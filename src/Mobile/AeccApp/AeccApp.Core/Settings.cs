// Helpers/Settings.cs
using Plugin.Settings;
using Plugin.Settings.Abstractions;
using System;

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

        private const string FirstChatKey = "First_chat";
        private static readonly bool FirstChatDefault = true;

        private const string HomeHeaderBannerAcceptClosedKey = "HomeHeaderBanner_closed";
        private static readonly bool HomeHeaderBannerAcceptClosedDefault = true;

        private const string FirstRequestLandingPageSeenKey = "FirstRequestLandingPage_seen";
        private static readonly bool FirstRequestLandingPageSeenDefault = true;

        private const string LastNewsCheckedKey = "Last_news_checked";
        private static readonly DateTime LastNewsCheckedDefault = DateTime.MinValue;

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

        public static bool FirstChat
        {
            get
            {
                return AppSettings.GetValueOrDefault(FirstChatKey, FirstChatDefault);
            }
            set
            {
                AppSettings.AddOrUpdateValue(FirstChatKey, value);
            }
        }

        /// <summary>
        /// returns bool with HomeHeaderBanner visibility current status
        /// </summary>
        public static bool HomeHeaderBannerClosed
        {
            get
            {
                return AppSettings.GetValueOrDefault(HomeHeaderBannerAcceptClosedKey, HomeHeaderBannerAcceptClosedDefault);
            }
            set
            {
                AppSettings.AddOrUpdateValue(HomeHeaderBannerAcceptClosedKey, value);
            }
        }    
        
        /// <summary>
        /// returns bool with FirstRequestLandingPage visibility current status
        /// </summary>
        public static bool FirstRequestLandingPageSeen
        {
            get
            {
                return AppSettings.GetValueOrDefault(FirstRequestLandingPageSeenKey, FirstRequestLandingPageSeenDefault);
            }
            set
            {
                AppSettings.AddOrUpdateValue(FirstRequestLandingPageSeenKey, value);
            }
        }

        public static DateTime LastNewsChecked
        {
            get
            {
                return AppSettings.GetValueOrDefault(LastNewsCheckedKey, LastNewsCheckedDefault);
            }
            set
            {
                AppSettings.AddOrUpdateValue(LastNewsCheckedKey, value);
            }
        }

        public static void Reset()
        {
            AppSettings.Remove(AccessTokenKey);
            AppSettings.Remove(TermsAndConditionsAcceptKey);
            AppSettings.Remove(FirstChatKey);
            AppSettings.Remove(HomeHeaderBannerAcceptClosedKey);
            AppSettings.Remove(FirstRequestLandingPageSeenKey);
        }
    }
}