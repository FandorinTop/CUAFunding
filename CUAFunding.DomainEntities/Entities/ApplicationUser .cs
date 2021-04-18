using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Identity;

namespace CUAFunding.DomainEntities.Entities
{
    public class ApplicationUser : IdentityUser<string>
    {
        public virtual List<Project> CreatedProjects { get; set; } = new List<Project>();
        public virtual List<Donation> Donations { get; set; } = new List<Donation>();
        public virtual List<Mark> Marks { get; set; } = new List<Mark>();
        public virtual List<ChatUser> Chats { get; set; } = new List<ChatUser>();

        public ApplicationUser()
        {
        }
    }
}
