using CUAFunding.Common.Extensions;
using CUAFunding.Configurations;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.SpaServices.AngularCli;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerUI;
using System;
using System.IO;

namespace CUAFunding
{

    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddAuthentication(config =>
            {
                config.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                config.DefaultChallengeScheme = OpenIdConnectDefaults.AuthenticationScheme;
            })
                .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddOpenIdConnect(OpenIdConnectDefaults.AuthenticationScheme, config =>
                {
                    config.Authority = "https://localhost:8000";
                    config.ClientId = "CUAFunding_id_client";
                    config.ClientSecret = "CUAFunding_id_secret_client";
                    config.SaveTokens = true;
                    config.RequireHttpsMetadata = true;
                    config.ResponseType = "code";
                    config.Scope.Add("offline_access");
                    config.TokenValidationParameters = new TokenValidationParameters
                    {
                        ClockSkew = TimeSpan.FromSeconds(500)
                    };
                });

            services.AddHttpClient();
            services.InjectDependencies(Configuration);
            services.InjectDataBase(Configuration);
            services.AddControllersWithViews()
                .AddJsonOptions(options => {
                // set this option to TRUE to indent the JSON output
                options.JsonSerializerOptions.WriteIndented = true;
                // set this option to NULL to use PascalCase instead of CamelCase (default)
                // options.JsonSerializerOptions.PropertyNamingPolicy = null;
            }); ;
            services.AddSpaStaticFiles(configuration =>
            {
                configuration.RootPath = "../CUAFunding.Angular/ClientApp/dist";
            });

            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo
                {
                    Description = "Swagger API v1",
                    Title = "Swagger",
                    Version = "1.0.0"
                });
            });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IServiceProvider serviceProvider)
        {
            if (env.IsDevelopment() || env.IsTesting())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles(new StaticFileOptions
            {
                FileProvider = new PhysicalFileProvider(Path.Combine(env.ContentRootPath, Configuration["ProjectRoot:DirectoryRoot"])), RequestPath= Configuration["ProjectRoot:RequestPath"]
            });
            if (!env.IsDevelopment() || env.IsTesting())
            {
                app.UseSpaStaticFiles();
            }

            app.UseRouting();

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseSwagger();

            app.UseSwaggerUI(options =>
            {
                options.DocumentTitle = "Title";
                options.RoutePrefix = "docs";
                options.SwaggerEndpoint("/swagger/v1/swagger.json", "Swagger UI");
                options.DocExpansion(DocExpansion.Full);
            });


            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller}/{action=Index}/{Id?}");
            });

            app.UseSpa(spa =>
            {
                spa.Options.SourcePath = "../CUAFunding.Angular/ClientApp";

                if (env.IsDevelopment() || env.IsTesting())
                {
                    spa.UseAngularCliServer(npmScript: "start");
                }
            });
        }
    }
}
