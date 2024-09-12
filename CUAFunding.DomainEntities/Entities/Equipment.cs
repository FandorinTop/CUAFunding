using CUAFunding.DomainEntities.Entities.Base;
using System.ComponentModel.DataAnnotations;

namespace CUAFunding.DomainEntities.Entities
{
    public class Equipment : BaseEntity
    {
        [MaxLength(64)]
        public string Name { get; set; }
    }
}
