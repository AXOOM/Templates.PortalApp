using System.IO;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SpaServices.AngularCli;
using Microsoft.DotNet.PlatformAbstractions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Swashbuckle.AspNetCore.Swagger;

namespace MyVendor.MyApp.Infrastructure
{
    public static class WebConfig
    {
        public static IServiceCollection AddWeb(this IServiceCollection services, IConfiguration config)
        {
            services.Configure<ForwardedHeadersOptions>(options => options.ForwardedHeaders = ForwardedHeaders.All);

            var identityOptions = Identity.GetOptions(config);
            services.AddSingleton(identityOptions);

            bool identityEnabled = identityOptions.Authority != null;

            if (identityEnabled)
                services.AddAuthentication(config);

            services.AddMvc(options =>
                     {
                         options.Filters.Add(typeof(ApiExceptionFilterAttribute));
                         if (identityEnabled)
                             options.AddAuthorizeFilter(identityOptions);
                     })
                    .SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            services.AddSpaStaticFiles(configuration => configuration.RootPath = "ClientApp/dist");

            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1",
                    new Info
                    {
                        Title = "My App",
                        Version = "v1",
                        Contact = new Contact
                        {
                            Email = "developer@example.com",
                            Url = "http://www.example.com"
                        }
                    });
                options.IncludeXmlComments(Path.Combine(ApplicationEnvironment.ApplicationBasePath, "MyVendor.MyApp.xml"));
                options.DescribeAllEnumsAsStrings();
                if (identityEnabled)
                    options.AddOAuth(identityOptions);
            });

            return services;
        }

        public static IApplicationBuilder UseWeb(this IApplicationBuilder app)
        {
			app.UseForwardedHeaders(); // must be first middleware in pipeline

            bool devMode = app.ApplicationServices.GetRequiredService<IHostingEnvironment>().IsDevelopment();
            if (devMode)
            {
                app.UseDeveloperExceptionPage()
                   .UseExceptionDemystifier()
                   .UseSwagger()
                   .UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "My App API v1"));
            }
            else
                app.UseExceptionHandler("/Error");

            app.UseAuthentication()
               .UseStaticFiles()
               .UseSpaStaticFiles();

            app.UseMvc(routes =>
                {
                    routes.MapRoute(
                        name: "default",
                        template: "{controller}/{action=Index}/{id?}");
                })
               .UseSpa(spa =>
                {
                    spa.Options.SourcePath = "ClientApp";
                    if (devMode)
                        spa.UseAngularCliServer(npmScript: "start");
                });

            return app;
        }
    }
}
