using System;
using CUAFunding.ViewModels.BaseViewModel;
using CUAFunding.ViewModels.BaseViewModel.Intefaces;

namespace CUAFunding.ViewModels.DonationViewModel
{
    public class EditDonationViewModel : BaseDonationViewModel, IBaseEditViewModel
    {
        public Guid Id { get; set; }
        public string UserName { get; set; }
        public string ProjectName { get; set; }
    }
}
