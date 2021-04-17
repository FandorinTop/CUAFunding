using CUAFunding.DomainEntities.Entities.Base;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CUAFunding.DomainEntities.Entities
{
    [Table("Marks")]
    public class Mark : BaseEntity
    {
        public string UserId { get; set; }
        [ForeignKey(nameof(UserId))]
        public virtual ApplicationUser User { get; set; }

        [Required]
        public string ProjectId { get; set; }

        [ForeignKey(nameof(ProjectId))]
        public virtual Project Project { get; set; }
        public int Value { get; set; }
        public Mark() : base()
        {
        }
    }
}
