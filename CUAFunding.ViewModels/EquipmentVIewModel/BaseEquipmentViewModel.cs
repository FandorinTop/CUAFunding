using System.ComponentModel.DataAnnotations;

namespace CUAFunding.ViewModels.EquipmentVIewModel
{
    public class BaseEquipmentViewModel
    {
        [Required]
        [MaxLength(64)]
        public string Name { get; set; }
    }
}
