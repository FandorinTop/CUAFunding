using CUAFunding.ViewModels.DonationViewModel;
using System.Threading.Tasks;

namespace CUAFunding.Interfaces.BussinessLogic.Services
{
    public interface IDonationService
    {
        public Task<bool> AddDonation(AddDonationViewModel viewModel);
    }
}
