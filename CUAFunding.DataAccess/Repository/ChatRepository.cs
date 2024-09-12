using CUAFunding.DomainEntities.Entities;
using CUAFunding.Interfaces.Repository;

namespace CUAFunding.DataAccess.Repository
{
    public class ChatRepository : BaseRepository<Chat>, IChatRepository
    {
        public ChatRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}
