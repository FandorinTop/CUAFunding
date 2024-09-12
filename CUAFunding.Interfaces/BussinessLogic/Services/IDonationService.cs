using CUAFunding.ViewModels;
using CUAFunding.ViewModels.DonationViewModel;
using System.Threading.Tasks;

namespace CUAFunding.Interfaces.BussinessLogic.Services
{
    public interface IDonationService
    {
        public Task<ApiResult<ShowDonationsViewModel>> GetApiResult(
               int pageIndex,
               int pageSize,
               string sortColumn = null,
               string sortOrder = null,
               string filterColumn = null,
               string filterQuery = null);

        public Task<bool> AddDonation(AddDonationViewModel viewModel);
    }
}
