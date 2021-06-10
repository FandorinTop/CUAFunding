using System;

namespace CUAFunding.ViewModels.BaseViewModel
{
    public class BaseDonationViewModel
    {
        public string UserId { get; set; }

        public string ProjectId { get; set; }

        public string Message { get; set; }

        public decimal Value { get; set; }
    }
}
