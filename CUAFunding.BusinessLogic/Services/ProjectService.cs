using CUAFunding.Common.Exceptions;
using CUAFunding.DomainEntities.Entities;
using CUAFunding.DomainEntities.Enums;
using CUAFunding.Interfaces.BussinessLogic.Providers;
using CUAFunding.Interfaces.BussinessLogic.Services;
using CUAFunding.Interfaces.Mappers;
using CUAFunding.Interfaces.Repository;
using CUAFunding.ViewModels;
using CUAFunding.ViewModels.ProjectViewModel;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Claims;
using System.Threading.Tasks;

namespace CUAFunding.BusinessLogic.Services
{
    public class ProjectService : IProjectService
    {
        private IProjectRepository _projectRepository;
        private IProjectMapper _projectMapper;
        private IHttpContextAccessor _httpContextAccessor;
        private IFileServerProvider _fileServerProvider;
        private IWebHostEnvironment _hostingEnvironment;


        public ProjectService(IProjectRepository projectRepository, IProjectMapper projectMapper, IHttpContextAccessor httpContextAccessor, IFileServerProvider fileServerProvider, IWebHostEnvironment hostingEnvironment)
        {
            _projectRepository = projectRepository;
            _projectMapper = projectMapper;
            _httpContextAccessor = httpContextAccessor;
            _fileServerProvider = fileServerProvider;
            _hostingEnvironment = hostingEnvironment;
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
            await CheckUserId(viewModel.Id, project);

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
            var result = _projectRepository
                .GetApiResult(pageIndex, pageSiez, sortColumn, sortOrder, filterColumn, filterQuery);

            return result;
        }

        public async Task ChangeProjectImage(Guid Id, IFormFile file)
        {
            var path = string.Empty;
            var project = await _projectRepository.Find(Id);
            var userId = new Guid(GetUserId());

            await CheckUserId(userId, project);

            if (project == null)
            {
                throw new Exception();
            }
            if (file != null)
            {
                path = await _fileServerProvider.LoadFilesAsync(Path.Combine(GetCurrentDirectory(), $"//{project.Title ?? "Unanamed"}//"), file, new List<EnalableFileExtensionTypes>() { EnalableFileExtensionTypes.jpg, EnalableFileExtensionTypes.jpeg, EnalableFileExtensionTypes.png });
            }
            if (!String.IsNullOrEmpty(project.ImagePath))
            {
                await _fileServerProvider.DeleteFileAsync(project.ImagePath);
            }

            project.ImagePath = path;
            await _projectRepository.Update(project);
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

        private string GetCurrentDirectory()
        {
            return _hostingEnvironment.ContentRootPath;
        }
    }
}
