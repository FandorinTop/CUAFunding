using CUAFunding.Common.Exceptions;
using CUAFunding.DomainEntities.Entities;
using CUAFunding.Interfaces.BussinessLogic.Services;
using CUAFunding.ViewModels;
using CUAFunding.ViewModels.ProjectViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CUAFunding.Controllers
{
    [Authorize]
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ProjectController : Controller
    {
        IProjectService _service;
        ILogger<ProjectController> _logger;

        public ProjectController(IProjectService service, ILogger<ProjectController> logger)
        {
            _logger = logger;
            _service = service;
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<ApiResult<ShowProjectViewModel>> ShowProject(
           int pageIndex = 0,
           int pageSize = 10,
           string sortColumn = null,
           string sortOrder = null,
           string filterColumn = null,
           string filterQuery = null)
        {
            var result = await _service.ShowProjects(pageIndex, pageSize, sortColumn, sortOrder, filterColumn, filterQuery);

            return result;
        }

        [AllowAnonymous]
        [HttpGet("{id}")]
        public async Task<ShowProjectViewModel> GetProject(Guid id)
        {
            var result = await _service.ShowProject(id);

            _logger.LogInformation($"Returned information about project with id: {id}");

            return result;
        }

        [HttpPut]
        public async Task<IActionResult> EditProject([FromBody] EditProjectViewModel viewModel)
        {
            try
            {
                await _service.EditProject(viewModel);
            }
            catch (MapperException ex)
            {
                _logger.LogWarning($"Fail project data update project with id: {viewModel.Id} ErrorMessage: {ex.Message}");
                return StatusCode(422, ex.Message + "\n" + ex.ValidationMessage);
            }

            _logger.LogInformation($"Successfull information update project with id: {viewModel.Id}");

            return Ok();
        }

        [HttpPost]
        public async Task<IActionResult> CreateProject([FromBody] CreateProjectViewModel viewModel)
        {
            Guid projId;
            try
            {
                projId = await _service.CreateProject(viewModel);
            }
            catch (MapperException ex)
            {
                _logger.LogWarning($"Fail create project ErrorMessage: {ex.Message}");
                return StatusCode(422, ex.Message);
            }

            _logger.LogInformation($"Successfull created  project with name: {viewModel.Title}");

            return Ok("Project Id: " + projId);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProject(Guid id)
        {
            try
            {
                await _service.DeleteProject(id);
            }
            catch (Exception ex)
            {
                _logger.LogInformation($"Fail delete project with id: {id}, ErrorMessage: {ex.Message}");
            }

            _logger.LogInformation($"Successful delete project with id: {id}");

            return Ok();
        }
    }
}
