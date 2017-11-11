using Aecc.Models;
using AeccApp.Core.Models;
using Microsoft.Identity.Client;
using Microsoft.Identity.Client.Internal;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AeccApp.Core.Services
{
    public class IdentityService : IIdentityService
    {
        private PublicClientApplication PCA = null;

        private const string VOLUNTEER_JOBTITLE = "voluntario";

        private string _token;

        private GlobalSetting GSetting { get { return GlobalSetting.Instance; } }

        private AzureAdB2COptions AdB2COptions { get { return GSetting.AzureAdB2COptions; } }

        public IdentityService()
        {
            PCA = new PublicClientApplication(AdB2COptions.ClientId, AdB2COptions.AuthoritySignUpSignIn)
            {
                RedirectUri = $"msal{AdB2COptions.ClientId}://auth"
            };
        }

        public async Task<bool> TryToLoginAsync(bool silentLogin)
        {
            bool result = false;
            try
            {
                AuthenticationResult ar = (silentLogin) ?
                                await PCA.AcquireTokenSilentAsync(AdB2COptions.FullNameScopes, GetUserByPolicy(PCA.Users, AdB2COptions.SignUpSignInPolicyId), AdB2COptions.AuthoritySignUpSignIn, true) :
                                await PCA.AcquireTokenAsync(AdB2COptions.FullNameScopes, App.UiParent);

                if (ar != null && ar.AccessToken != null)
                {
                    UpdateUserData(ar);

                    result = true;
                }
            }
            catch (MsalException ex)
            {
                // Checking the exception message 
                // should ONLY be done for B2C
                // reset and not any other error.
                if (ex.Message.Contains("AADB2C90118"))
                    await PasswordResetAsync();

                else if (ex.ErrorCode != MsalError.UserMismatch)
                    LogOff();
            }
            return result;
        }

        public void LogOff()
        {
            GSetting.User = null;
            foreach (var user in PCA.Users)
            {
                PCA.Remove(user);
            }
        }

        public async Task EditProfileAsync()
        {
            try
            {
                // KNOWN ISSUE:
                // User will get prompted 
                // to pick an IdP again.
                AuthenticationResult ar = await PCA.AcquireTokenAsync(AdB2COptions.FullNameScopes, GetUserByPolicy(PCA.Users, AdB2COptions.EditProfilePolicyId), UIBehavior.SelectAccount, string.Empty, null, AdB2COptions.AuthorityEditProfile, App.UiParent);
                UpdateUserData(ar);
            }
            catch (Exception ex)
            {
                // Alert if any exception excludig user cancelling sign-in dialog
                //if (((ex as MsalException)?.ErrorCode != "authentication_canceled"))
                //  await DisplayAlert($"Exception:", ex.ToString(), "Dismiss");
            }
        }

        public async Task<string> GetTokenAsync()
        {
            try
            {
                if (string.IsNullOrEmpty(_token))
                {
                    AuthenticationResult ar = await PCA.AcquireTokenSilentAsync(AdB2COptions.FullNameScopes, GetUserByPolicy(PCA.Users, AdB2COptions.SignUpSignInPolicyId), AdB2COptions.AuthoritySignUpSignIn, false);
                    _token = ar.AccessToken;
                }
            }
            catch (MsalUiRequiredException ex)
            {
                Debug.WriteLine(ex);
                //  await DisplayAlert($"Session has expired, please sign out and back in.", ex.ToString(), "Dismiss");
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                // await DisplayAlert($"Exception:", ex.ToString(), "Dismiss");
            }
            return _token;
        }


        private async Task PasswordResetAsync()
        {
            try
            {
                AuthenticationResult ar = await PCA.AcquireTokenAsync(AdB2COptions.FullNameScopes, (IUser)null, UIBehavior.SelectAccount, string.Empty, null, AdB2COptions.AuthorityPasswordReset, App.UiParent);
                UpdateUserData(ar);
            }
            catch (Exception ex)
            {
                // Alert if any exception excludig user cancelling sign-in dialog
                //if (((ex as MsalException)?.ErrorCode != "authentication_canceled"))
                //    await DisplayAlert($"Exception:", ex.ToString(), "Dismiss");
            }
        }

        private void UpdateUserData(AuthenticationResult ar)
        {
            _token = ar.AccessToken;
            var user = ParseIdToken(ar.IdToken);
            GSetting.User = new UserData()
            {
                UserId = ar.UniqueId,
                Name = ar.User.Name,
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

        
    }
}
