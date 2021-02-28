using System;
using System.ComponentModel.DataAnnotations.Schema;
using CUAFunding.DomainEntities.Entities.Base;

namespace CUAFunding.DomainEntities.Entities
{
    public class Donation : BaseEntity
    {
        public Guid? UserId { get; set; }
        [ForeignKey(nameof(UserId))]
        public virtual ApplicationUser User { get; set; }
        public Guid ProjectId { get; set; }

        [ForeignKey(nameof(ProjectId))]
        public virtual Project Project { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal Value { get; set; }
        public string Message { get; set; }
        public Donation() : base()
        {
        }
    }
}
