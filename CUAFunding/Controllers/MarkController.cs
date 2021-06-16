using CUAFunding.Common.Exceptions;
using CUAFunding.Interfaces.BussinessLogic.Services;
using CUAFunding.ViewModels;
using CUAFunding.ViewModels.FileUploadingViewModel;
using CUAFunding.ViewModels.MarkViewModel;
using CUAFunding.ViewModels.ProjectViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace CUAFunding.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class MarkController : Controller
    {
        IMarkService _service;
        ILogger<MarkController> _logger;

        public MarkController(IMarkService service, ILogger<MarkController> logger)
        {
            _logger = logger;
            _service = service;
        }

        [HttpPut]
        public async Task<IActionResult> EditMark(EditMarkViewModel viewModel)
        {
            try
            {
                await _service.UpdateMark(viewModel);
            }
            catch (Exception ex)
            {
                _logger.LogWarning($"Fail project data update mark in project with id: {viewModel.Id} ErrorMessage: {ex.Message}");
                throw ex;
            }

            _logger.LogInformation($"Successfull information update mark in project with id: {viewModel.Id}");

            return Ok(true);
        }

        [HttpPost]
        public async Task<IActionResult> CreateMark(CreateMarkViewModel viewModel)
        {
            try
            {
                await _service.AddMark(viewModel);
            }
            catch (Exception ex)
            {
                _logger.LogWarning($"Fail create mark ErrorMessage: {ex.Message}");
                throw ex;
            }

            _logger.LogInformation($"Successfull added mark to: {viewModel.ProjectId}");

            return Ok(true);
        }
    }
}
