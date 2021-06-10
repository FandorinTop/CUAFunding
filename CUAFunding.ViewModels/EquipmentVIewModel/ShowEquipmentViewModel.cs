using System;

namespace CUAFunding.ViewModels.EquipmentVIewModel
{
    public class ShowEquipmentViewModel : BaseEquipmentViewModel
    {
        public string Id { get; set; }

        public DateTime CreationTime { get; set; }

        public DateTime LastEditionTime { get; set; }
    }
}
