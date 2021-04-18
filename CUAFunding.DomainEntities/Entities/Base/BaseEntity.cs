using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace CUAFunding.DomainEntities.Entities.Base
{
    public class BaseEntity
    {
        public BaseEntity()
        {
            CreationDate = DateTime.UtcNow;
            LastEditDate = DateTime.UtcNow;
            Id = Guid.NewGuid().ToString();
        }
        [Key]
        public string Id { get; set; }

        public DateTime CreationDate { get; set; }

        public DateTime LastEditDate { get; set; }
    }
}
