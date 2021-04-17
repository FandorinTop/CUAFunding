using System;
using System.Collections.Generic;
using System.Text;

namespace CUAFunding.ViewModels.BaseViewModel.ViewItems
{
    public class DonationViewItem 
    {
        public string Id { get; set; }
        public string UserId { get; set; }
        public string ProjectId { get; set; }
        public decimal Value { get; set; }
        public string Message { get; set; }
        public string UserName { get; set; }
    }
}
