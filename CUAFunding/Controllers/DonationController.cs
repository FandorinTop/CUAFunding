using CUAFunding.Common.Exceptions;
using CUAFunding.Interfaces.BussinessLogic.Services;
using CUAFunding.ViewModels.DonationViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
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

        [HttpPost]
        public async Task<IActionResult> AddDonation([FromBody] AddDonationViewModel viewModel)
        {
            try
            {
                await _service.AddDonation(viewModel);
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
