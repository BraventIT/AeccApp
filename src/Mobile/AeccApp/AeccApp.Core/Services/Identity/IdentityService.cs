using AeccApp.Core.Models;
using Microsoft.Identity.Client;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AeccApp.Core.Services
{
    public class IdentityService : IIdentityService
    {
        private PublicClientApplication PCA = null;

        private const string VOLUNTEER_JOBTITLE = "voluntario";

        private const string PolicySignUpSignIn = "B2C_1_inicioDeSesion";
        private const string PolicyEditProfile = "B2C_1_perfilxamarin";
        private const string PolicyResetPassword = "B2C_1_passxamarin";

        private string[] Scopes
        {
            get { return new string[] { $"{GSetting.BaseEndpoint}/demo.read" }; }
        }

        private string AuthorityBase
        {
            get { return $"https://login.microsoftonline.com/tfp/{GSetting.DomainName}/"; }
        }

        private string Authority
        {
            get { return $"{AuthorityBase}{PolicySignUpSignIn}"; }
        }
        private string AuthorityEditProfile
        {
            get { return $"{AuthorityBase}{PolicyEditProfile}"; }
        }
        private string AuthorityPasswordReset
        {
            get { return $"{AuthorityBase}{PolicyResetPassword}"; }
        }

        private string _token;

        private GlobalSetting GSetting { get { return GlobalSetting.Instance; } }

        public IdentityService()
        {
            PCA = new PublicClientApplication(GSetting.ApplicationID, Authority)
            {
                RedirectUri = $"msal{GSetting.ApplicationID}://auth"
            };
        }

        public async Task<bool> TryToLoginAsync(bool silentLogin)
        {
            bool result = false;
            AuthenticationResult ar = (silentLogin) ?
                            await PCA.AcquireTokenSilentAsync(Scopes, GetUserByPolicy(PCA.Users, PolicySignUpSignIn), Authority, true) :
                            await PCA.AcquireTokenAsync(Scopes, App.UiParent);

            if (ar != null)
            {
                UpdateUserData(ar);

                result = true;
            }
            return result;
        }

        private void UpdateUserData(AuthenticationResult ar)
        {
            _token = ar.AccessToken;
            var user = ParseIdToken(ar.IdToken);
            GSetting.User = new UserData()
            {
                UserId = ar.UniqueId,
                UserName = ar.User.Name,
                FirstName = user["given_name"]?.ToString(),
                Surname = user["family_name"]?.ToString(),
                Email = user["emails"]?.First().ToString(),
            };


            if (user["extension_Age"] != null)
            {
                GSetting.User.Age = (int)user["extension_Age"];
            }

            string jobTitle = user["jobTitle"]?.ToString() ?? string.Empty;
            GSetting.User.IsVolunteer = string.Compare(jobTitle, VOLUNTEER_JOBTITLE, StringComparison.CurrentCultureIgnoreCase) == 0;
        }

        public void LogOff()
        {
            GSetting.User = null;
            foreach (var user in PCA.Users)
            {
                PCA.Remove(user);
            }
        }

        public async Task<string> GetTokenAsync()
        {
            try
            {
                if (string.IsNullOrEmpty(_token))
                {
                    AuthenticationResult ar = await PCA.AcquireTokenSilentAsync(Scopes, GetUserByPolicy(PCA.Users, PolicySignUpSignIn), Authority, false);
                    _token = ar.AccessToken;
                }
            }
            catch (MsalUiRequiredException ex)
            {
                //  await DisplayAlert($"Session has expired, please sign out and back in.", ex.ToString(), "Dismiss");
            }
            catch (Exception ex)
            {
                // await DisplayAlert($"Exception:", ex.ToString(), "Dismiss");
            }
            return _token;
        }


        private JObject ParseIdToken(string idToken)
        {
            // Get the piece with actual user info
            idToken = idToken.Split('.')[1];
            idToken = Base64UrlDecode(idToken);
            return JObject.Parse(idToken);
        }

        private IUser GetUserByPolicy(IEnumerable<IUser> users, string policy)
        {
            foreach (var user in users)
            {
                string userIdentifier = Base64UrlDecode(user.Identifier.Split('.')[0]);
                if (userIdentifier.EndsWith(policy, StringComparison.CurrentCultureIgnoreCase)) return user;
            }

            return null;
        }

        private string Base64UrlDecode(string s)
        {
            s = s.Replace('-', '+').Replace('_', '/');
            s = s.PadRight(s.Length + (4 - s.Length % 4) % 4, '=');
            var byteArray = Convert.FromBase64String(s);
            var decoded = Encoding.UTF8.GetString(byteArray, 0, byteArray.Count());
            return decoded;
        }

        public async Task EditProfileAsync()
        {
            try
            {
                // KNOWN ISSUE:
                // User will get prompted 
                // to pick an IdP again.
                AuthenticationResult ar = await App.PCA.AcquireTokenAsync(App.Scopes, GetUserByPolicy(App.PCA.Users, App.PolicyEditProfile), UIBehavior.SelectAccount, string.Empty, null, App.AuthorityEditProfile, App.UiParent);
                UpdateUserData(ar);
            }
            catch (Exception ex)
            {
                // Alert if any exception excludig user cancelling sign-in dialog
                //if (((ex as MsalException)?.ErrorCode != "authentication_canceled"))
                  //  await DisplayAlert($"Exception:", ex.ToString(), "Dismiss");
            }
        }
    }
}
