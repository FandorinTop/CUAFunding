using CUAFunding.ViewModels.BaseViewModel;
using CUAFunding.ViewModels.BaseViewModel.ViewItems;
using System.Collections.Generic;

namespace CUAFunding.ViewModels.DonationViewModel
{
    public class ShowDonationsViewModel : BaseDonationViewModel
    {
        public string ProjectName { get; set; }

        public string CreationDate { get; set; }
    }
}
