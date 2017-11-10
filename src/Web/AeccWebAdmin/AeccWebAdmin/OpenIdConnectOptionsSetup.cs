﻿using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Identity.Client;
using System.Security.Claims;
using AeccApi.WebAdmin.Models;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using System.Net;
using Aecc.Models;

namespace AeccApi.WebAdmin
{
    public class OpenIdConnectOptionsSetup : IConfigureOptions<OpenIdConnectOptions>
    {

        public OpenIdConnectOptionsSetup(IOptions<AzureAdB2COptions> b2cOptions)
        {
            AzureAdB2COptions = b2cOptions.Value;
        }

        public AzureAdB2COptions AzureAdB2COptions { get; set; }

        public void Configure(OpenIdConnectOptions options)
        {
            options.ClientId = AzureAdB2COptions.ClientId;
            options.Authority = AzureAdB2COptions.AuthoritySignUpSignIn;
            options.UseTokenLifetime = true;
            options.TokenValidationParameters = new TokenValidationParameters() { NameClaimType = "name" };

            options.Events = new OpenIdConnectEvents()
            {
                OnRedirectToIdentityProvider = OnRedirectToIdentityProvider,
                OnRemoteFailure = OnRemoteFailure,
                OnAuthorizationCodeReceived = OnAuthorizationCodeReceived
            };
        }

        public Task OnRedirectToIdentityProvider(RedirectContext context)
        {
            var defaultPolicy = AzureAdB2COptions.DefaultPolicy;
            if (context.Properties.Items.TryGetValue(AzureAdB2COptions.PolicyAuthenticationProperty, out var policy) &&
                !policy.Equals(defaultPolicy))
            {
                context.ProtocolMessage.Scope = OpenIdConnectScope.OpenIdProfile;
                context.ProtocolMessage.ResponseType = OpenIdConnectResponseType.IdToken;
                context.ProtocolMessage.IssuerAddress = context.ProtocolMessage.IssuerAddress.ToLower().Replace(defaultPolicy.ToLower(), policy.ToLower());
                context.Properties.Items.Remove(AzureAdB2COptions.PolicyAuthenticationProperty);
            }
            else if (!string.IsNullOrEmpty(AzureAdB2COptions.AppID))
            {
                context.ProtocolMessage.Scope += $" offline_access {AzureAdB2COptions.Scope}";
                context.ProtocolMessage.ResponseType = OpenIdConnectResponseType.CodeIdToken;
            }
            return Task.FromResult(0);
        }

        public Task OnRemoteFailure( RemoteFailureContext context)
        {
            context.HandleResponse();
            // Handle the error code that Azure AD B2C throws when trying to reset a password from the login page 
            // because password reset is not supported by a "sign-up or sign-in policy"
            if (context.Failure is OpenIdConnectProtocolException && context.Failure.Message.Contains("AADB2C90118"))
            {
                // If the user clicked the reset password link, redirect to the reset password route
                context.Response.Redirect("/Session/ResetPassword");
            }
            else if (context.Failure is OpenIdConnectProtocolException && context.Failure.Message.Contains("access_denied"))
            {
                context.Response.Redirect("/");
            }
            else
            {
                context.Response.Redirect("/Home/Error?message=" + WebUtility.UrlEncode(context.Failure.Message));
            }
            return Task.FromResult(0);
        }

        public async Task OnAuthorizationCodeReceived(AuthorizationCodeReceivedContext context)
        {
            // Use MSAL to swap the code for an access token
            // Extract the code from the response notification
            var code = context.ProtocolMessage.Code;

            string signedInUserID = context.Principal.FindFirst(ClaimTypes.NameIdentifier).Value;
            TokenCache userTokenCache = new MSALSessionCache(signedInUserID, context.HttpContext).GetMsalCacheInstance();
            ConfidentialClientApplication cca = new ConfidentialClientApplication(AzureAdB2COptions.ClientId, AzureAdB2COptions.AuthoritySignUpSignIn, AzureAdB2COptions.RedirectUri, new ClientCredential(AzureAdB2COptions.ClientSecret), userTokenCache, null);
            try
            {
                AuthenticationResult result = await cca.AcquireTokenByAuthorizationCodeAsync(code, AzureAdB2COptions.FullNameScopes);


                context.HandleCodeRedemption(result.AccessToken, result.IdToken);
            }
            catch (Exception ex)
            {
                //TODO: Handle
                throw;
            }
        }
    }
}
