using CUAFunding.DomainEntities.Entities.Base;
using NetTopologySuite.Geometries;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CUAFunding.DomainEntities.Entities
{
    public class Project : BaseEntity
    {
        [Required]
        public string OwnerId { get; set; }
        [ForeignKey(nameof(OwnerId))]
        public virtual ApplicationUser Owner { get; set; }
        [MaxLength(30)]
        public string Title { get; set; }
        [MaxLength(600)]
        public string Description { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal Goal { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal Сollected { get; set; }
        public int PageVisitorsCount { get; set; }
        public string ImagePath { get; set; }
        public DateTime ExpirationDate { get; set; }
        public Point Location { get; set; }

        public virtual IEnumerable<Donation> Donations { get; set; }
        public virtual IEnumerable<Mark> Marks { get; set; }
        public Project() : base()
        {
        }
    }
}
