using CUAFunding.Common.Exceptions;
using CUAFunding.Common.Mappers;
using CUAFunding.DomainEntities.Entities;
using CUAFunding.Interfaces.BussinessLogic.Services;
using CUAFunding.Interfaces.Mappers;
using CUAFunding.Interfaces.Repository;
using CUAFunding.ViewModels;
using CUAFunding.ViewModels.ProjectViewModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace CUAFunding.BusinessLogic.Services
{
    public class ProjectService : IProjectService
    {
        private IProjectRepository _projectRepository;
        private IProjectMapper _projectMapper;
        private IHttpContextAccessor _httpContextAccessor;

        public ProjectService(IProjectRepository projectRepository, IProjectMapper projectMapper, IHttpContextAccessor httpContextAccessor)
        {
            _projectRepository = projectRepository;
            _projectMapper = projectMapper;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<Guid> CreateProject(CreateProjectViewModel viewModel)
        {
            var userId = GetUserId();
            viewModel.OwnerId = new Guid(userId);
            if (String.IsNullOrEmpty(userId))
            {                
                throw new AuthorizationException("User is not authorize");
            }

            var project = _projectMapper.Create(viewModel);
            await _projectRepository.Insert(project);

            return project.Id;
        }

        public async Task DeleteProject(Guid Id)
        {
            var project = await _projectRepository.Find(Id);
            var res = CheckUserId(Id, project);

            await _projectRepository.Delete(Id);
        }

        public async Task EditProject(EditProjectViewModel viewModel)
        {
            var userId = GetUserId();
            viewModel.OwnerId = new Guid(userId);

            var project = await _projectRepository.Find(viewModel.Id);
            var res = CheckUserId(viewModel.Id, project);

            project = _projectMapper.Edit(project, viewModel);
            await _projectRepository.Update(project);
        }

        public async Task<ShowProjectViewModel> ShowProject(Guid Id)
        {
            var project = await _projectRepository.Find(Id);
            var collected = await _projectRepository.GetCollected(Id);
            var rating = await _projectRepository.GetRanting(Id);

            var res = _projectMapper.Show(project, collected, rating);
            
            return res;
        }

        public Task<ApiResult<ShowProjectViewModel>> ShowProjects(int pageIndex, int pageSiez, string sortColumn = null, string sortOrder = null, string filterColumn = null, string filterQuery = null)
        {
            var userId = GetUserId();
            var result = _projectRepository.GetApiResult(pageIndex, pageSiez, sortColumn, sortOrder, filterColumn, filterQuery);

            return result;
        }

        private Task CheckUserId(Guid Id, Project project)
        {
            var userId = GetUserId();

            if (project == null)
            {
                throw new Exception("No project with such id");
            }
            if (project.OwnerId.ToString() != userId)
            {
                throw new Exception("User is not project owner");
            }

            return Task.CompletedTask;
        }
        private string GetUserId()
        {
            return _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
        }
    }
}
