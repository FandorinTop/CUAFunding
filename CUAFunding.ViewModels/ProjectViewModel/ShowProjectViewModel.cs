using System;
using System.Collections.Generic;
using System.Text;
using CUAFunding.ViewModels.BaseViewModel.ViewItems;

using CUAFunding.ViewModels.BaseViewModel;
using CUAFunding.ViewModels.BaseViewModel.Intefaces;

namespace CUAFunding.ViewModels.ProjectViewModel
{
    public class ShowProjectViewModel : BaseProjectViewModel, IBaseEditViewModel
    {
        public IEnumerable<DonationViewItem> Donations;
        public string Id { get; set; }
        public decimal? Collected { get; set; }
        public int PageVisitorsCount { get; set; }
        public double? AvgRating { get; set; }

        public ShowProjectViewModel()
        {
            Donations = new List<DonationViewItem>();
        }
    }
}
