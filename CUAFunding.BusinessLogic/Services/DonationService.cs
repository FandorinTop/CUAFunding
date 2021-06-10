using CUAFunding.DataAccess;
using CUAFunding.DomainEntities.Entities;
using CUAFunding.Interfaces.BussinessLogic.Services;
using CUAFunding.ViewModels.DonationViewModel;
using System.Threading.Tasks;

namespace CUAFunding.BusinessLogic.Services
{
    public class DonationService : IDonationService
    {
        private readonly ApplicationDbContext _context;

        public async Task<bool> AddDonation(AddDonationViewModel viewModel)
        {
            var project = await _context.Projects.FindAsync(viewModel.ProjectId);

            var user = await _context.ApplicationUsers.FindAsync(viewModel.UserId);

            var donation = new Donation()
            {
                Message = viewModel.Message,
                Project = project,
                User = user,
                Value = viewModel.Value
            };

            await _context.Donations.AddAsync(donation);
            var result = await _context.SaveChangesAsync();

            return result >= 0 ? true : false;
        }
    }
}
