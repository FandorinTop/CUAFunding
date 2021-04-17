using CUAFunding.BusinessLogic.Providers;
using CUAFunding.BusinessLogic.Services;
using CUAFunding.Common.Mappers;
using CUAFunding.DataAccess;
using CUAFunding.DataAccess.Repository;
using CUAFunding.DomainEntities.Entities;
using CUAFunding.Interfaces.BussinessLogic.Providers;
using CUAFunding.Interfaces.BussinessLogic.Services;
using CUAFunding.Interfaces.Mappers;
using CUAFunding.Interfaces.Repository;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace CUAFunding.Configurations
{
    public static class DependenciesConfig
    {
        public static void InjectDependencies(this IServiceCollection service, IConfiguration configuration)
        {
            service.AddHttpContextAccessor();

            #region Mappers
            service.AddTransient<IProjectMapper, ProjectMapper>();
            service.AddTransient<IMarkMapper, MarkMapper>();
            service.AddTransient<IDonationMapper, DonationMapper>();
            #endregion

            #region Repositories
            service.AddTransient<IProjectRepository, ProjectRepository>();
            service.AddTransient<IDonationRepository, DonationRepository>();
            service.AddTransient<IMarkRepository, MarkRepository>();
            #endregion

            service.AddTransient<RoleManager<ApplicationRole>>();
            service.AddTransient<SignInManager<ApplicationUser>>();
            service.AddTransient<UserManager<ApplicationUser>>();

            #region Services
            service.AddTransient<IAccountService, AccountService>();
            service.AddTransient<IProjectService, ProjectService>();
            #endregion

            #region Providers
            service.AddTransient<IFileServerProvider, FileServerProvider>();
            #endregion
        }
    }
}
