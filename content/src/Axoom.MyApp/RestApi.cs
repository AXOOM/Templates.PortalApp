using System.IO;
using Axoom.MyApp.Pipeline;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.SpaServices.Webpack;
using Microsoft.DotNet.PlatformAbstractions;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using Swashbuckle.AspNetCore.Swagger;

namespace Axoom.MyApp
{
    public static class RestApi
    {
        public static IServiceCollection AddRestApi(this IServiceCollection services)
        {
            services
                .AddMvc(options =>
                {
                    options.Filters.Add(typeof(ApiExceptionFilterAttribute));
                })
                .AddJsonOptions(options =>
                {
                    options.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
                    options.SerializerSettings.Converters.Add(new StringEnumConverter {CamelCaseText = true});
                });

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
            });

            return services;
        }

        public static IApplicationBuilder UseRestApi(this IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "My Service API v1"));
                app.UseWebpackDevMiddleware(new WebpackDevMiddlewareOptions {HotModuleReplacement = true});
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
            app.UseFileServer(enableDirectoryBrowsing: env.IsDevelopment());

            return app;
        }
    }
}