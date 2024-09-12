using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CUAFunding.DomainEntities.Entities.Base;

namespace CUAFunding.DomainEntities.Entities
{
    public class Donation : BaseEntity
    {
        public string UserId { get; set; }
        [ForeignKey(nameof(UserId))]
        public virtual ApplicationUser User { get; set; }

        [Required]
        public string ProjectId { get; set; }

        [ForeignKey(nameof(ProjectId))]
        public virtual Project Project { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal Value { get; set; }
        public string Message { get; set; }
        public string Email { get; set; }

        public Donation() : base()
        {
        }
    }
}
