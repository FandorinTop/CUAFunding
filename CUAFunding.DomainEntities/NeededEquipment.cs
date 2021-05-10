using CUAFunding.DomainEntities.Entities;
using CUAFunding.DomainEntities.Entities.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace CUAFunding.DomainEntities
{
    public class NeededEquipment : BaseEntity
    {
        public string ProjectId { get; set; }

        public Project Project { get; set; }

        public string EquipmentId { get; set; }

        public Equipment Equipment { get; set; }

        public bool IsRequired { get; set; }
    }
}
