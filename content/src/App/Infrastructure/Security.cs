using System.Collections.Generic;
using IdentityServer4.AccessTokenValidation;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Swashbuckle.AspNetCore.Swagger;

namespace MyVendor.MyApp.Infrastructure
{
    public static class Security
    {
        public static IServiceCollection AddSecurity(this IServiceCollection services, IConfiguration configuration)
        {
            var authenticationOptions = configuration.ToAuthenticationOptions();
            services.AddSingleton(authenticationOptions);
            if (string.IsNullOrEmpty(authenticationOptions.Authority))
                return services;

            services.AddAuthentication(IdentityServerAuthenticationDefaults.AuthenticationScheme)
                    .AddIdentityServerAuthentication(configuration.Bind);

            services.Configure<MvcOptions>(x => x.Filters.Add(new AuthorizeFilter(ScopePolicy.Create(authenticationOptions.ApiName))));

            services.ConfigureSwaggerGen(options =>
            {
                options.AddSecurityDefinition("oauth2", new OAuth2Scheme
                {
                    Type = "oauth2",
                    Flow = "implicit",
                    AuthorizationUrl = authenticationOptions.Authority + "/connect/authorize",
                    Scopes = new Dictionary<string, string>
                    {
                        [authenticationOptions.ApiName] = "Query the app."
                    }
                });
                options.AddSecurityRequirement(new Dictionary<string, IEnumerable<string>>
                {
                    ["oauth2-implicit"] = new string[0]
                });
            });

            return services;
        }

        public static IApplicationBuilder UseSecurity(this IApplicationBuilder app)
        {
            var provider = app.ApplicationServices;
            var options = provider.GetRequiredService<IdentityServerAuthenticationOptions>();

            if (string.IsNullOrEmpty(options.Authority))
                provider.GetRequiredService<ILogger<Startup>>().LogWarning("Security is disabled.");
            else
                app.UseAuthentication();

            return app;
        }

        public static void AddAuthorizeFilter(this MvcOptions options, [CanBeNull] IConfiguration authenticationConfiguration)
        {
            var authenticationOptions = authenticationConfiguration?.ToAuthenticationOptions();
            if (string.IsNullOrEmpty(authenticationOptions?.Authority))
                return;

            options.Filters.Add(new AuthorizeFilter(ScopePolicy.Create(authenticationOptions.ApiName + ".api")));
        }

        private static IdentityServerAuthenticationOptions ToAuthenticationOptions(this IConfiguration configuration)
        {
            var identityOptions = new IdentityServerAuthenticationOptions();
            configuration.Bind(identityOptions);
            return identityOptions;
        }
    }
}
