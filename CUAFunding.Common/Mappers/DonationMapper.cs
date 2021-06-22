﻿using System;
using System.Collections.Generic;
using System.Text;
using CUAFunding.DomainEntities.Entities;
using CUAFunding.ViewModels.DonationViewModel;
using CUAFunding.ViewModels.BaseViewModel.ViewItems;
using System.Linq;
using CUAFunding.Interfaces.Mappers;

namespace CUAFunding.Common.Mappers
{
    public class DonationMapper : IDonationMapper
    {
        public Donation Create(AddDonationViewModel viewModel)
        {
            Donation donation = new Donation();
            donation.UserId = viewModel.UserId;
            donation.ProjectId = viewModel.ProjectId;
            donation.Value = viewModel.Value;
            donation.Message = viewModel.Message;

            return donation;
        }

        public Donation Edit(Donation donation,EditDonationViewModel viewModel)
        {
            donation.UserId = viewModel.UserId;
            donation.ProjectId = viewModel.ProjectId;
            donation.Value = viewModel.Value;
            donation.Message = viewModel.Message;

            return donation;
        }

        public EditDonationViewModel Edit(Donation donation)
        {
            EditDonationViewModel viewModel = new EditDonationViewModel();
            viewModel.UserId = donation.UserId;
            viewModel.ProjectId = donation.ProjectId;
            viewModel.UserId = donation.UserId;
            viewModel.Value = donation.Value;
            viewModel.UserName = donation.User.UserName;
            viewModel.ProjectName = donation.Project.Title;
            viewModel.Message = donation.Message;

            return viewModel;
        }

        public IEnumerable<ShowDonationsViewModel> Show(IEnumerable<Donation> donations)
        {
            IEnumerable<ShowDonationsViewModel> viewModel;

            viewModel = donations.Select(viewItem => new ShowDonationsViewModel()
            {
                ProjectId = viewItem.ProjectId,
                UserId = viewItem.UserId,
                Value = viewItem.Value,
                Message = viewItem.Message,
            });

            return viewModel;
        }
    }
}
