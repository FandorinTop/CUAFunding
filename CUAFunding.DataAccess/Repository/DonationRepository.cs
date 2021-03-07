using CUAFunding.DomainEntities.Entities;
using CUAFunding.Interfaces.Repository;

namespace CUAFunding.DataAccess.Repository
{
    public class DonationRepository : BaseRepository<Donation>, IDonationRepository
    {
        public DonationRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}
