using CUAFunding.Common.Exceptions;
using CUAFunding.Interfaces.BussinessLogic.Services;
using CUAFunding.ViewModels;
using CUAFunding.ViewModels.DonationViewModel;
using CUAFunding.ViewModels.ProjectViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Security.Claims;
using System.Threading.Tasks;

namespace CUAFunding.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class DonationController : Controller
    {
        IDonationService _service;
        ILogger<DonationController> _logger;

        public DonationController(IDonationService service, ILogger<DonationController> logger)
        {
            _logger = logger;
            _service = service;
        }
        [AllowAnonymous]
        [HttpGet]
        public async Task<ApiResult<ShowDonationsViewModel>> ShowDonations(
           int pageIndex = 0,
           int pageSize = 10,
           string sortColumn = null,
           string sortOrder = null,
           string filterColumn = null,
           string filterQuery = null)
        {
            var result = await _service.GetApiResult(pageIndex, pageSize, sortColumn, sortOrder, filterColumn, filterQuery);

            return result;
        }

        [HttpPost]
        public async Task<IActionResult> AddDonation([FromBody] AddDonationViewModel viewModel)
        {
            var userId = GetUserId();

            viewModel.UserId = userId ?? viewModel.UserId;

            try
            {
                await _service.AddDonation(viewModel);
            }
            catch (MapperException ex)
            {
                _logger.LogWarning($"Fail create donation ErrorMessage: {ex.Message}");
                return StatusCode(422, ex.Message + "\n" + ex.ValidationMessage);
            }

            _logger.LogInformation($"Successfull added donation to: {viewModel.ProjectId}");

            return Ok(true);
        }

        private string GetUserId()
        {
            return User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "TEST";
        }
    }
}
