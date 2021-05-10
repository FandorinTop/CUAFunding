using CUAFunding.DomainEntities.Entities.Base;
using System.ComponentModel.DataAnnotations.Schema;

namespace CUAFunding.DomainEntities.Entities
{
    public class ImagePath : BaseEntity
    {
        public string Path { get; set; }

        [ForeignKey(nameof(Project))]
        public int ProjectId { get; set; }

        public virtual Project Project { get; set; }
    }
}
