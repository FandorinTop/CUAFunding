using CUAFunding.DataAccess;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using CUAFunding.DomainEntities.Entities;
using IdentityServer4.EntityFramework.DbContexts;
using CUAFunding.Interfaces.BussinessLogic.Services;
using Microsoft.AspNetCore.Builder;
using CUAFunding.BusinessLogic.Services;
using CUAFunding.Interfaces.Repository;

namespace CUAFunding.Configurations
{
    public static partial class DBContextConfig
    {
        public static void InjectDataBase(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("Testing"))
                .UseLazyLoadingProxies());

            //TODO remove this
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

        public static void InitProjectTempData(this IApplicationBuilder app)
        {
            using (var serviceScope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope())
            {
                var projectService = serviceScope.ServiceProvider.GetRequiredService<IProjectRepository>();
                var userManager = serviceScope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
                var temp = userManager.FindByNameAsync("Admin").GetAwaiter().GetResult();

                foreach (var item in InitProject(temp.Id))
                {
                    projectService.Insert(item);
                }
            }
        }

        private static IEnumerable<Project> InitProject(string OwnerId)
        {
            yield return new Project()
            {
                CreationDate = DateTime.UtcNow,
                OwnerId = OwnerId,
                Description = "1111111",
                ExpirationDate = DateTime.UtcNow.AddDays(10),
                Title = "111111",
                Goal = 111111,
                ImagePath = "",
                PageVisitorsCount = 111111
            };

            yield return new Project()
            {
                CreationDate = DateTime.UtcNow,
                OwnerId = OwnerId,
                Description = "222222",
                ExpirationDate = DateTime.UtcNow.AddDays(20),
                Title = "222222",
                Goal = 222222,
                ImagePath = "",
                PageVisitorsCount = 222222
            };

            yield return new Project()
            {
                CreationDate = DateTime.UtcNow,
                OwnerId = OwnerId,
                Description = "333333",
                ExpirationDate = DateTime.UtcNow.AddDays(30),
                Title = "333333",
                Goal = 333333,
                ImagePath = "",
                PageVisitorsCount = 333333
            };
        }
    }
}
