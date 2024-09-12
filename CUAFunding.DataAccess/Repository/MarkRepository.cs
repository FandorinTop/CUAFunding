using CUAFunding.DomainEntities.Entities;
using CUAFunding.Interfaces.Repository;

namespace CUAFunding.DataAccess.Repository
{
    public class MarkRepository : BaseRepository<Mark>, IMarkRepository
    {
        public MarkRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}
