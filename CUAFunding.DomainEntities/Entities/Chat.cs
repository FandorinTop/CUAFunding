using CUAFunding.DomainEntities.Entities.Base;
using System.Collections.Generic;

namespace CUAFunding.DomainEntities.Entities
{
    public class Chat : BaseEntity
    {
        public bool IsPublic { get; set; } = true;

        public virtual List<ChatUser> Users { get; set; } = new List<ChatUser>();
    }
}
