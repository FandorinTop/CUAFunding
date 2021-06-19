using CUAFunding.BusinessLogic.Services;
using CUAFunding.DataAccess;
using CUAFunding.DomainEntities.Entities;
using CUAFunding.Interfaces.BussinessLogic.Services;
using IdentityModel;
using IdentityServer4;
using IdentityServer4.EntityFramework.DbContexts;
using IdentityServer4.EntityFramework.Mappers;
using IdentityServer4.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NetTopologySuite;
using System.Security.Claims;
using IdentityServer4.Services;

namespace CUAFunding.IdentityServer
{
    public static class IdentityServerConfigureService
    {
        public static IEnumerable<Client> GetClients()
        {
            
            yield return new Client()
            {
                ClientId = "CUAFunding_id_spa",
                RequireClientSecret=false,              
                RequirePkce=true,
                AllowedGrantTypes = GrantTypes.Code,
                AllowedScopes =
                {
                    "roles",
                    "ProductAPI",
                    "Read",
                    "Write",
                    "Delete",
                    IdentityServerConstants.StandardScopes.Profile,
                    IdentityServerConstants.StandardScopes.OpenId,
                },
                RedirectUris = { "https://localhost:5001/signin-oidc" },
                PostLogoutRedirectUris = { "https://localhost:5001/signout-callback-oidc" },
                RequireConsent = false,
                AccessTokenLifetime = 3000,
                AllowOfflineAccess = true,

            };

            yield return new Client()
            {
                ClientId = "CUAFunding_id_chat",
                ClientSecrets = { new Secret("CUAFunding_id_secret_chat".ToSha256()) },
                RequirePkce = true,
                AllowedGrantTypes = GrantTypes.Code,
                AllowedScopes =
                {   
                    "roles",
                    "ProductAPI",
                    "Read",
                    "Write",
                    "Delete",
                    IdentityServerConstants.StandardScopes.Profile,
                    IdentityServerConstants.StandardScopes.OpenId,
                },
                RedirectUris = { "https://localhost:44377" },
                PostLogoutRedirectUris = { "https://localhost:44377" },
                RequireConsent = false,
                AccessTokenLifetime = 3000,
                AllowOfflineAccess = true,
            };
            yield return new Client()
            {
                ClientId = "CUAFunding_id_client",
                ClientSecrets = { new Secret("CUAFunding_id_secret_client".ToSha256()) },
                AllowedGrantTypes = GrantTypes.Code,
                AllowedScopes =
                {
                    "roles",
                    "ProductAPI",
                    "Read",
                    "Write",
                    "Delete",
                    IdentityServerConstants.StandardScopes.Profile,
                    IdentityServerConstants.StandardScopes.OpenId,
                },
                RedirectUris = { "https://localhost:5001/signin-oidc" },
                PostLogoutRedirectUris = { "https://localhost:5001/signout-callback-oidc" },
                RequireConsent = false,
                AccessTokenLifetime = 3000,
                AllowOfflineAccess = true,
            };
            yield return new Client()
            {
                ClientId = "xamarin",
                RequirePkce = true,
                RequireClientSecret = false,
                AllowedGrantTypes = GrantTypes.Code,
                AllowedScopes =
                {
                    "roles",
                    "ProductAPI",
                    "Read",
                    "Write",
                    "Delete",
                    IdentityServerConstants.StandardScopes.Profile,
                    IdentityServerConstants.StandardScopes.OpenId,
                },
                RedirectUris = { "xamarinformsclients://callback" },
                RequireConsent = false,
                AllowAccessTokensViaBrowser = true,
                AccessTokenLifetime = 3000,
                AllowOfflineAccess = true,
            };
        }
        public static IEnumerable<ApiResource> GetApiResources()
        {
            yield return new ApiResource("ProductAPI");
        }

        public static IEnumerable<IdentityResource> GetIdentityResources()
        {
            yield return new IdentityResources.OpenId();
            yield return new IdentityResources.Profile();
            yield return new IdentityResource
            {
                Name = "roles",
                DisplayName = "Roles",
                UserClaims = { JwtClaimTypes.Role }
            };
        }

        public static IEnumerable<ApiScope> GetApiScopes()
        {
            yield return new ApiScope("Read", "Read data");
            yield return new ApiScope("Write", "Write data");
            yield return new ApiScope("Delete", "Delete data");
            yield return new ApiScope("Admin", "Admin data");
            yield return new ApiScope("Admin", "Admin data");
        }

        public static void InjectIdentityServer(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddTransient<IProfileService, ProfileService>();
            services.AddTransient<RoleManager<ApplicationRole>>();
            services.AddTransient<SignInManager<ApplicationUser>>();
            services.AddTransient<UserManager<ApplicationUser>>();

            services.AddTransient<IAccountService,AccountService>();

            var connectionString = configuration.GetConnectionString("Testing");
            var migrationsAssembly = "CUAFunding.DataAccess";
            
            services.AddDbContext<ApplicationDbContext>(options =>
                           options.UseSqlServer(connectionString,
                           x=> x.UseNetTopologySuite()));

            services.AddIdentity<ApplicationUser, ApplicationRole>(config =>
            {
                config.Password.RequireDigit = false;
                config.Password.RequireLowercase = false;
                config.Password.RequireNonAlphanumeric = false;
                config.Password.RequireUppercase = false;
                config.Password.RequiredLength = 6;
            })
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            services.AddIdentityServer()
                .AddDeveloperSigningCredential()
                .AddAspNetIdentity<ApplicationUser>()
                .AddConfigurationStore(option =>
                {
                    option.ConfigureDbContext = builder => builder.UseSqlServer(connectionString,
                        sql => sql.MigrationsAssembly(migrationsAssembly));
                })

                .AddOperationalStore(options =>
                {
                    options.ConfigureDbContext = builder =>
                        builder.UseSqlServer(connectionString,
                            sql => sql.MigrationsAssembly(migrationsAssembly));

                    options.EnableTokenCleanup = true;
                    options.TokenCleanupInterval = 300;

                });
          
            services.AddAuthentication()
                .AddGoogle("Google", config =>
                {
                    config.SignInScheme = IdentityServerConstants.ExternalCookieAuthenticationScheme;

                    config.ClientId = "1072607496895-sg4osbuo1nr3lnv7t2lenj89pfklvctn.apps.googleusercontent.com";
                    config.ClientSecret = "afaohp9YskbDTRayonH5I-4T";
                });


        }

        public static void InitializeIdentityServerDatabase(this IApplicationBuilder app)
        {
            using (var serviceScope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope())
            {
                serviceScope.ServiceProvider.GetRequiredService<PersistedGrantDbContext>().Database.Migrate();
                var context = serviceScope.ServiceProvider.GetRequiredService<ConfigurationDbContext>();
                context.Database.Migrate();

                var userManager = serviceScope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
                var roleManager = serviceScope.ServiceProvider.GetRequiredService<RoleManager<ApplicationRole>>();


                var amin =  userManager.FindByNameAsync("SuperAdmin1").GetAwaiter().GetResult();

                if (!roleManager.RoleExistsAsync("Admin").GetAwaiter().GetResult())
                {
                    var roleResult = roleManager.CreateAsync(new ApplicationRole("Admin") { Id = Guid.NewGuid().ToString() }).GetAwaiter().GetResult();
                }
                if (amin==null)
                {
                    CreateAdminUser(userManager).GetAwaiter().GetResult();
                }
                if (!context.Clients.Any())
                {
                    foreach (var client in IdentityServerConfigureService.GetClients())
                    {
                        context.Clients.Add(client.ToEntity());
                    }
                    context.SaveChanges();
                }
                if (!context.IdentityResources.Any())
                {
                    foreach (var resource in IdentityServerConfigureService.GetIdentityResources())
                    {
                        context.IdentityResources.Add(resource.ToEntity());
                    }
                    context.SaveChanges();
                }
                if (!context.ApiResources.Any())
                {
                    foreach (var resource in IdentityServerConfigureService.GetApiResources())
                    {
                        context.ApiResources.Add(resource.ToEntity());
                    }
                    context.SaveChanges();
                }
            }
        }

        private async static Task CreateAdminUser(UserManager<ApplicationUser> userManager)
        {
            var email = "SUPERADMIN1@admin.com";
            var password = "ADMINIDENTITY!123aa";
            var user = new ApplicationUser();
            user.UserName = "SuperAdmin1";
            user.NormalizedEmail = email.ToUpper();
            user.NormalizedUserName = email.ToUpper();
            user.Email = email;
            user.EmailConfirmed = true;
            user.PhoneNumber = "+380996330749";
            user.EmailConfirmed = true;

            var result = await userManager.CreateAsync(user, password);

            ApplicationUser applicationUser = await userManager.FindByNameAsync(user.UserName);

            await userManager.AddClaimAsync(applicationUser, new Claim(JwtClaimTypes.Role, "Admin"));
            await userManager.AddToRoleAsync(applicationUser, "Admin");
        }
    }
}
