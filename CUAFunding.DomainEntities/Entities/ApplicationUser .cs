using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using CUAFunding.DomainEntities.Entities.Base;
using Microsoft.AspNetCore.Identity;

namespace CUAFunding.DomainEntities.Entities
{
    public class ApplicationUser : IdentityUser<Guid>
    {
        public virtual IEnumerable<Project> CreatedProjects { get; set; }
        public virtual IEnumerable<Donation> Donations { get; set; }
        public virtual IEnumerable<Mark> Marks { get; set; }
        public ApplicationUser()
        {
            CreatedProjects = new List<Project>();
            Donations = new List<Donation>();
            Marks = new List<Mark>();
        }
    }
}
