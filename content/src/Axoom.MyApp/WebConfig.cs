using System.Collections.Generic;
using System.IO;
using Axoom.MyApp.Pipeline;
using IdentityServer4.AccessTokenValidation;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.SpaServices.Webpack;
using Microsoft.DotNet.PlatformAbstractions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using Swashbuckle.AspNetCore.Swagger;

namespace Axoom.MyApp
{
    public static class WebConfig
    {
        public static IServiceCollection AddWeb(this IServiceCollection services, IConfiguration config)
        {
            string identityServerUri = GetIdentityServerUri(config, out string apiName, out string apiSecret);

            services
                .AddMvc(options =>
                {
                    options.Filters.Add(typeof(ApiExceptionFilterAttribute));
                    if (identityServerUri != null)
                        options.Filters.Add(new AuthorizeFilter(ScopePolicy.Create(apiName)));
                })
                .AddJsonOptions(options =>
                {
                    options.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
                    options.SerializerSettings.Converters.Add(new StringEnumConverter {CamelCaseText = true});
                });

            if (identityServerUri != null)
            {
                services
                    .AddAuthentication(IdentityServerAuthenticationDefaults.AuthenticationScheme)
                    .AddIdentityServerAuthentication(options =>
                    {
                        options.Authority = identityServerUri;
                        options.ApiName = apiName;
                        options.ApiSecret = apiSecret;
                        options.RequireHttpsMetadata = false;
                    });
            }

            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1",
                    new Info
                    {
                        Title = "My App",
                        Version = "v1",
                        Contact = new Contact
                        {
                            Email = "developer@axoom.com",
                            Name = "AXOOM GmbH",
                            Url = "http://developer.axoom.com"
                        }
                    });
                options.IncludeXmlComments(Path.Combine(ApplicationEnvironment.ApplicationBasePath, "Axoom.MyApp.xml"));
                options.DescribeAllEnumsAsStrings();
                if (identityServerUri != null)
                {
                    options.AddSecurityDefinition("oauth2", new OAuth2Scheme
                    {
                        Type = "oauth2",
                        Flow = "implicit",
                        AuthorizationUrl = identityServerUri + "/connect/authorize",
                        Scopes = new Dictionary<string, string>
                        {
                            [apiName] = "Query the app."
                        }
                    });
                }
            });

            return services;
        }

        [CanBeNull]
        private static string GetIdentityServerUri(IConfiguration config, out string apiName, out string apiSecret)
        {
            var identityConfig = config.GetSection("Identity");
            apiName = identityConfig["ApiName"];
            apiSecret = identityConfig["ApiSecret"];

            return config.GetValue<string>("IDENTITY_SERVER_URI");
        }

        public static IApplicationBuilder UseWeb(this IApplicationBuilder app)
        {
            bool devMode = app.ApplicationServices.GetRequiredService<IHostingEnvironment>().IsDevelopment();
            if (devMode)
            {
                app
                    .UseDeveloperExceptionPage()
                    .UseSwagger()
                    .UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "My Service API v1"))
                    .UseWebpackDevMiddleware(new WebpackDevMiddlewareOptions {HotModuleReplacement = true});
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseMvc(x => x
                .MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}")
                .MapSpaFallbackRoute(
                    name: "spa-fallback",
                    defaults: new {controller = "Home", action = "Index"}));
            app.UseFileServer(enableDirectoryBrowsing: devMode);

            return app;
        }
    }
}