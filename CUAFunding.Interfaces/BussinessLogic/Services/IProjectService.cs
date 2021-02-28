using CUAFunding.ViewModels;
using CUAFunding.ViewModels.ProjectViewModel;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CUAFunding.Interfaces.BussinessLogic.Services
{
    public interface IProjectService
    {
        Task<ShowProjectViewModel> ShowProject(Guid id);
        Task<ApiResult<ShowProjectViewModel>> ShowProjects(
            int pageIndex,
            int pageSiez,
            string sortColumn = null,
            string sortOrder = null,
            string filterColumn = null,
            string filterQuery = null);
        Task<Guid> CreateProject(CreateProjectViewModel viewModel);
        Task EditProject(EditProjectViewModel viewModel);
        Task ChangeProjectImage(Guid Id, IFormFile file);
        Task DeleteProject(Guid Id);
    }
}
