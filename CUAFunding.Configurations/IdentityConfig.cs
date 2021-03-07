using CUAFunding.DataAccess;
using CUAFunding.DomainEntities.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CUAFunding.Configurations
{
    public static class IdentityConfig
    {
        public static void InjectIdentity(this IServiceCollection services, IConfiguration configuration)
        {
            
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

            services.ConfigureApplicationCookie(option =>
            {
                option.LoginPath = "/Account/Login";
                option.LoginPath = "/Account/AccessDenied";
            });
        }

        public async static Task InitializeDB(this IServiceProvider serviceProvider)
        {
            var roleManager = serviceProvider.GetRequiredService<RoleManager<ApplicationRole>>();
            var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            IdentityResult roleResult;

            bool adminRoleCheck = await roleManager.RoleExistsAsync("Admin");
            if (!adminRoleCheck)
            {
                roleResult = await roleManager.CreateAsync(new ApplicationRole("Admin"));
                await CreateAdminUser(userManager);
            }
        }

        private async static Task CreateAdminUser(UserManager<ApplicationUser> userManager)
        {
            var email = "Admin123@admin.com";
            var password = "Admin123!";
            var user = new ApplicationUser();
            user.UserName = "Admin";
            user.NormalizedEmail = email.ToUpper();
            user.NormalizedUserName = email.ToUpper();
            user.Email = email;
            user.EmailConfirmed = true;
            user.PhoneNumber = "+380996330749";
            user.EmailConfirmed = true;

            await userManager.CreateAsync(user, password);
            ApplicationUser applicationUser = await userManager.FindByNameAsync(user.UserName);

            await userManager.AddToRoleAsync(applicationUser, "Admin");
        }
    }
}
