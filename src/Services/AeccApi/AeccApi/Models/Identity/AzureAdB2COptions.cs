namespace Aecc.Models
{
    public class AzureAdB2COptions
    {
        public const string PolicyAuthenticationProperty = "Policy";
        private const string AzureAdB2CInstance = "https://login.microsoftonline.com/tfp";


        public string ClientId { get; set; }
        public string Tenant { get; set; }
        public string SignUpSignInPolicyId { get; set; }
        public string ResetPasswordPolicyId { get; set; }
        public string EditProfilePolicyId { get; set; }
        public string RedirectUri { get; set; }

        public string DefaultPolicy => SignUpSignInPolicyId;

        public string AuthoritySignUpSignIn => $"{AzureAdB2CInstance}/{Tenant}/{SignUpSignInPolicyId}/v2.0/";
        public string AuthorityEditProfile => $"{AzureAdB2CInstance}/{Tenant}/{EditProfilePolicyId}/v2.0/";
        public string AuthorityPasswordReset => $"{AzureAdB2CInstance}/{Tenant}/{ResetPasswordPolicyId}/v2.0/";

        public string ClientSecret { get; set; }

        public string AppID { get; set; }
        public string Scope { get; set; }

        public string[] FullNameScopes
        {
            get
            {
                return new string[] { $"https://{Tenant}/{AppID}/{Scope}" };
            }
        }
    }
}
