using Microsoft.AspNetCore.Identity;
using System;

namespace CUAFunding.DomainEntities.Entities
{
    public class ApplicationRole : IdentityRole<Guid>
    {
        public DateTime CreationDate { get; set; }
        public ApplicationRole(string name) : base(name)
        {
            CreationDate = DateTime.UtcNow;
        }
    }
}
