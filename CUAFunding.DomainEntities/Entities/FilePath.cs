using CUAFunding.DomainEntities.Entities.Base;
using System.ComponentModel.DataAnnotations.Schema;

namespace CUAFunding.DomainEntities.Entities
{
    public class FilePath : BaseEntity
    {
        public string FileName { get; set; }

        public string FileDestination { get; set; }

        public string Path { get; set; }

        [ForeignKey(nameof(Project))]
        public string ProjectId { get; set; }

        public virtual Project Project { get; set; }
    }
}
