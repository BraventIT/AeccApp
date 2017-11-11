using Aecc.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;
using System.Text;
using System.Threading.Tasks;

namespace AeccApi.Extensions
{
    public static class AzureAdB2CAuthenticationBuilderExtensions
    {
        public static AuthenticationBuilder AddAzureAdB2C(this AuthenticationBuilder builder, Action<AzureAdB2COptions> configureOptions)
        {
            builder.Services.Configure(configureOptions);
            builder.Services.AddSingleton<IConfigureOptions<JwtBearerOptions>, JwtBearerOptionsSetup>();
            builder.AddJwtBearer();
            return builder;
        }

        public class JwtBearerOptionsSetup : IConfigureNamedOptions<JwtBearerOptions>
        {

            public JwtBearerOptionsSetup(IOptions<AzureAdB2COptions> b2cOptions)
            {
                AzureAdB2COptions = b2cOptions.Value;
            }

            public AzureAdB2COptions AzureAdB2COptions { get; set; }

            public void Configure(string name, JwtBearerOptions jwtOptions)
            {
                jwtOptions.Authority = AzureAdB2COptions.AuthoritySignUpSignIn;
                jwtOptions.Audience = AzureAdB2COptions.ClientId;
                jwtOptions.Events = new JwtBearerEvents
                {
                    OnAuthenticationFailed = AuthenticationFailed
                };
            }

            public void Configure(JwtBearerOptions options)
            {
                Configure(Options.DefaultName, options);
            }

            private Task AuthenticationFailed(AuthenticationFailedContext arg)
            {
                // For debugging purposes only!
                var s = $"AuthenticationFailed: {arg.Exception.Message}";
                arg.Response.ContentLength = s.Length;
                arg.Response.Body.Write(Encoding.UTF8.GetBytes(s), 0, s.Length);
                return Task.FromResult(0);
            }
        }
    }
}
