using CUAFunding.Common.Extensions;
using CUAFunding.Configurations;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.SpaServices.AngularCli;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using CUAFunding.Configurations;
using System;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.IdentityModel.Logging;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using IdentityServer4;

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
                    //config.SignOutScheme = IdentityServerConstants.SignoutScheme;
                    //config.SignInScheme = IdentityServerConstants.ExternalCookieAuthenticationScheme;

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
            services.AddControllersWithViews();
            services.AddSpaStaticFiles(configuration =>
            {
                configuration.RootPath = "../CUAFunding.Angular/ClientApp/dist";
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
            app.UseStaticFiles();
            if (!env.IsDevelopment() || env.IsTesting())
            {
                app.UseSpaStaticFiles();
            }

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();


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
