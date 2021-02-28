using System;
using System.Collections.Generic;
using System.Text;

namespace CUAFunding.ViewModels.BaseViewModel.ViewItems
{
    public class DonationViewItem 
    {
        public Guid Id { get; set; }
        public Guid? UserId { get; set; }
        public Guid ProjectId { get; set; }
        public decimal Value { get; set; }
        public string Message { get; set; }
        public string UserName { get; set; }
    }
}