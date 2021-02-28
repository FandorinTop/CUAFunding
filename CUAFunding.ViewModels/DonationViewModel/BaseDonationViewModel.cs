using System;

namespace CUAFunding.ViewModels.BaseViewModel
{
    public class BaseDonationViewModel
    {
        public Guid? UserId { get; set; }
        public Guid ProjectId { get; set; }
        public string Message { get; set; }
        public decimal Value { get; set; }
    }
}
