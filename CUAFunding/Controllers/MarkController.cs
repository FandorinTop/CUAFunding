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
        public async Task<IActionResult> EditMark([FromBody] EditMarkViewModel viewModel)
        {
            try
            {
                await _service.UpdateMark(viewModel);
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
        public async Task<IActionResult> CreateMark([FromBody] CreateMarkViewModel viewModel)
        {
            try
            {
                await _service.AddMark(viewModel);
            }
            catch (MapperException ex)
            {
                _logger.LogWarning($"Fail create mark ErrorMessage: {ex.Message}");
                return StatusCode(422, ex.Message + "\n" + ex.ValidationMessage);
            }

            _logger.LogInformation($"Successfull added mark to: {viewModel.ProjectId}");

            return Ok(true);
        }
    }
}
