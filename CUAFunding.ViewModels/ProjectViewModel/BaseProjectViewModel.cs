using Microsoft.AspNetCore.Http;
using System;

namespace CUAFunding.ViewModels.BaseViewModel
{
    public class BaseProjectViewModel
    {
        public string OwnerId { get; set; }

        public string Title { get; set; }
        
        public string Description { get; set; }

        public decimal Goal { get; set; }

        public double? LocationX { get; set; }

        public double? LocationY { get; set; }

        public DateTime ExpirationDate { get; set; }
    }
}
