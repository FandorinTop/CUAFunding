using CUAFunding.ViewModels.BaseViewModel.ViewItems;
using System.Collections.Generic;

namespace CUAFunding.ViewModels.DonationViewModel
{
    public class ShowDonationsViewModel
    {
        public IEnumerable<DonationViewItem> Donations { get; set; }

        public ShowDonationsViewModel()
        {
            Donations = new List<DonationViewItem>();
        }
    }
}
