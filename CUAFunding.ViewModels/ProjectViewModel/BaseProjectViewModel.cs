using System;

namespace CUAFunding.ViewModels.BaseViewModel
{
    public class BaseProjectViewModel
    {
        public string OwnerId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public decimal Goal { get; set; }
        public string ImagePath { get; set; }
        public DateTime ExpirationDate { get; set; }
    }
}
