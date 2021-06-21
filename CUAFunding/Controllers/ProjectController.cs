﻿using CUAFunding.Common.Exceptions;
using CUAFunding.DomainEntities.Entities;
using CUAFunding.Interfaces.BussinessLogic.Services;
using CUAFunding.ViewModels;
using CUAFunding.ViewModels.FileUploadingViewModel;
using CUAFunding.ViewModels.ProjectViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CUAFunding.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
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
        public async Task<ShowProjectViewModel> GetProject(string id)
        {
            var result = await _service.ShowProject(id);

            _logger.LogInformation($"Returned information about project with id: {id}");

            return result;
        }

        [HttpPut("{id}")]
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

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> CreateProject([FromBody] CreateProjectViewModel viewModel)
        {
            string projId;
            try
            {
                projId = await _service.CreateProject(viewModel);
            }
            catch (MapperException ex)
            {
                _logger.LogWarning($"Fail create project ErrorMessage: {ex.Message}");
                return StatusCode(422, ex.Message + "\n" + ex.ValidationMessage);
            }

            _logger.LogInformation($"Successfull created  project with name: {viewModel.Title}");

            return Ok();
            return Ok("Project Id: " + projId);
        }

        //[AllowAnonymous]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProject(string id)
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

        [Route("ChangeProjectImage")]
        [HttpPost]
        public async Task<IActionResult> ChangeProjectImage([FromForm] FileUploading viewModel)
        {
            try
            {
                await _service.ChangeProjectImage(viewModel.Id, viewModel.Picture);
            }
            catch (UploadingException ex)
            {
                _logger.LogWarning("Fail update file with id: " + viewModel.Id + "Exception message: " + ex.Message);
                return StatusCode(422, ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError("Fail update file with id: " + viewModel.Id + "Exception message: " + ex.Message);
                return StatusCode(500);
            }

            _logger.LogInformation("Update file Image with id: " + viewModel.Id);

            return Ok();
        }

        [HttpGet("UserProject")]
        public async Task<ApiResult<ShowProjectViewModel>> UserProjects(
           int pageIndex = 0,
           int pageSize = 10,
           string sortColumn = null,
           string sortOrder = null,
           string filterColumn = null,
           string filterQuery = null,
           string userId = null)
        {
            return await _service.UserProjects(pageIndex, pageSize, sortColumn, sortOrder, filterColumn, filterQuery, userId);
        }
    }
}
