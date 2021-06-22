using CUAFunding.DomainEntities.Entities;
using CUAFunding.ViewModels.DonationViewModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace CUAFunding.Interfaces.Mappers
{
    public interface IDonationMapper
    {
        public Donation Create(AddDonationViewModel viewModel);
        public Donation Edit(Donation donation, EditDonationViewModel viewModel);
        public EditDonationViewModel Edit(Donation donation);
        public IEnumerable<ShowDonationsViewModel> Show(IEnumerable<Donation> donations);
    }
}
