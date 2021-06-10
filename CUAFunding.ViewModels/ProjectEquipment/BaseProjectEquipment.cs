using System.Collections.Generic;

namespace CUAFunding.ViewModels.ProjectEquipment
{
    public class BaseEquipmentItem
    {
        public string Id { get; set; }

        public bool IsRequired { get; set; }

        public EntityStateViewModel EntityState { get; set; }
    }

    public class BaseProjectEquipment
    {
        public string Id { get; set; }

        public IEnumerable<BaseEquipmentItem> EquipmentIds { get; set; } = new List<BaseEquipmentItem>();
    }
}
