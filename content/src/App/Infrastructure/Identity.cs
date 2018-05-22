using System.Collections.Generic;
using IdentityServer4.AccessTokenValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace MyVendor.MyApp.Infrastructure
{
    public static class Identity
    {
        public static IdentityServerAuthenticationOptions GetOptions(IConfiguration config)
        {
            var options = new IdentityServerAuthenticationOptions();
            Bind(config, options);
            return options;
        }

        private static void Bind(IConfiguration config, IdentityServerAuthenticationOptions options)
        {
            options.Authority = config.GetValue<string>("IDENTITY_SERVER_URI");
            config.Bind("Identity", options);
        }

        public static void AddAuthentication(this IServiceCollection services, IConfiguration config)
            => services.AddAuthentication(IdentityServerAuthenticationDefaults.AuthenticationScheme)
                       .AddIdentityServerAuthentication(options => Bind(config, options));

        public static void AddAuthorizeFilter(this MvcOptions options, IdentityServerAuthenticationOptions identityOptions)
            => options.Filters.Add(new AuthorizeFilter(ScopePolicy.Create(identityOptions.ApiName)));

        public static void AddOAuth(this SwaggerGenOptions options, IdentityServerAuthenticationOptions identityOptions)
            => options.AddSecurityDefinition("oauth2", new OAuth2Scheme
            {
                Type = "oauth2",
                Flow = "implicit",
                AuthorizationUrl = identityOptions.Authority + "/connect/authorize",
                Scopes = new Dictionary<string, string>
                {
                    [identityOptions.ApiName] = "Query the app."
                }
            });
    }
}
