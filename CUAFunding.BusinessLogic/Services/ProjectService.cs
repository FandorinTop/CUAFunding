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
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace CUAFunding.BusinessLogic.Services
{
    public class ProjectService : IProjectService
    {
        #region Fields
        private IProjectRepository _projectRepository;
        private IProjectMapper _projectMapper;
        private IHttpContextAccessor _httpContextAccessor;
        private IFileServerProvider _fileServerProvider;
        private IConfiguration _configuration;
        #endregion

        public ProjectService(IProjectRepository projectRepository, IProjectMapper projectMapper, IHttpContextAccessor httpContextAccessor, IFileServerProvider fileServerProvider, IConfiguration configuration)
        {
            _projectRepository = projectRepository;
            _projectMapper = projectMapper;
            _httpContextAccessor = httpContextAccessor;
            _fileServerProvider = fileServerProvider;
            _configuration = configuration;
        }

        public async Task<string> CreateProject(CreateProjectViewModel viewModel)
        {
            var userId = GetUserId();
            viewModel.OwnerId = userId;
            if (String.IsNullOrEmpty(userId))
            {
                throw new AuthorizationException("User is not authorize");
            }

            var project = _projectMapper.Create(viewModel);
            await _projectRepository.Insert(project);

            return project.Id;
        }

        public async Task DeleteProject(string Id)
        {
            var project = await _projectRepository.Find(Id);
            var (isCorrectUser, message) = CheckUserId(Id, project);
            if (!isCorrectUser)
            {
                throw new AuthorizationException(message);
            }

            await _projectRepository.Delete(Id);
        }

        public async Task EditProject(EditProjectViewModel viewModel)
        {
            var userId = GetUserId();
            viewModel.OwnerId = userId;
            var project = await _projectRepository.Find(viewModel.Id);

            //var (isCorrectUser, message) = CheckUserId(viewModel.OwnerId, project);
            //if (!isCorrectUser)
            //{
            //    throw new AuthorizationException(message);
            //}

            project = _projectMapper.Edit(project, viewModel);
            await _projectRepository.Update(project);
        }

        public async Task<ShowProjectViewModel> ShowProject(string Id)
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

        public async Task ChangeProjectImage(string Id, IFormFile file)
        {
            var path = string.Empty;
            var project = await _projectRepository.Find(Id);
            var userId = GetUserId();

            var (isCorrectUser, message)= CheckUserId(userId, project);
            if (!isCorrectUser)
            {
                throw new AuthorizationException(message);
            }

            if (file != null)
            {
                path = await _fileServerProvider.LoadFilesAsync(Path.Combine(GetCurrentDirectory(), $"{project.Title ?? "Unanamed"}"), file, new List<EnalableFileExtensionTypes>() { EnalableFileExtensionTypes.jpg, EnalableFileExtensionTypes.jpeg, EnalableFileExtensionTypes.png });
            }
            if (!String.IsNullOrEmpty(project.MainImagePath))
            {
                await _fileServerProvider.DeleteFileAsync(project.MainImagePath);
            }

            project.MainImagePath = path;
            await _projectRepository.Update(project);
        }

        public async Task<ApiResult<ShowProjectViewModel>> UserProjects(int pageIndex, int pageSiez, string sortColumn = null, string sortOrder = null, string filterColumn = null, string filterQuery = null, string userId = null)
        {
            var id = GetUserId();

            if(id is null)
            {
                id = userId;
            }

            var projects = await _projectRepository.GetApiResult(pageIndex, pageSiez, sortColumn, sortOrder, filterColumn, filterQuery, id);

            return projects;
        }

        private (bool, string) CheckUserId(string Id, Project project)
        {
            var userId = GetUserId();

            if (project == null)
            {
                return (false, "No project with such id");
            }
            if (project.OwnerId.ToString() != userId)
            {
                return (false, "User is not project owner");
            }

            return (true, string.Empty);
        }


        private string GetUserId()
        {
            return _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "TEST";
        }

        private string GetCurrentDirectory()
        {
            return _configuration["ProjectRoot:DirectoryRoot"];
        }
    }
}
