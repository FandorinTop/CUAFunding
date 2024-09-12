using CUAFunding.Common.Helpers;
using CUAFunding.DataAccess;
using CUAFunding.DomainEntities.Entities;
using CUAFunding.Interfaces.BussinessLogic.Services;
using CUAFunding.ViewModels;
using CUAFunding.ViewModels.DonationViewModel;
using CUAFunding.ViewModels.ProjectViewModel;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Linq.Dynamic;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;

namespace CUAFunding.BusinessLogic.Services
{
    public class DonationService : IDonationService
    {
        Type entityType = typeof(Donation);

        private readonly ApplicationDbContext _context;

        public async Task<ApiResult<ShowDonationsViewModel>> GetApiResult(
            int pageIndex,
            int pageSize,
            string sortColumn = null,
            string sortOrder = null,
            string filterColumn = null,
            string filterQuery = null)
        {
            var source = _context.Donations.Select(proj => new ShowDonationsViewModel()
            {                
                Message = proj.Message,
                UserId = proj.UserId,
                ProjectId = proj.ProjectId,
                Email = proj.Email,
                Value = proj.Value,
                CreationDate = proj.CreationDate.ToShortDateString().ToString(),
            });

            if (!String.IsNullOrEmpty(filterColumn) && !String.IsNullOrEmpty(filterQuery) && entityType.IsValidProperty(filterColumn))
            {
                source = source.Where(String.Format("{0}.Contains(@0)", filterColumn), filterQuery);
            }

            if (!String.IsNullOrEmpty(sortColumn) && entityType.IsValidProperty(sortColumn))
            {
                var sortingString = String.Empty;

                if (!string.IsNullOrEmpty(sortColumn)
                       && entityType.IsValidProperty(sortColumn))
                {
                    sortOrder = !string.IsNullOrEmpty(sortOrder)
                        && sortOrder.ToUpper() == "ASC"
                        ? "ASC"
                        : "DESC";

                    if (string.IsNullOrEmpty(sortingString))
                    {
                        sortingString += $"{sortColumn} {sortOrder}";
                    }
                }

                source = source.OrderBy(sortingString);
            }

            var skipedSource = source.Skip(pageIndex * pageSize).Take(pageSize);
            var count = await source.CountAsync();

            var data = await skipedSource.ToListAsync();

            foreach (var item in data)
            {
                item.ProjectName = ((await _context.Projects.FirstOrDefaultAsync(proj => proj.Id == item.ProjectId))?.Title) ?? "Project removed";
                item.UserId = item.UserId ?? "Secret";
                item.Email = item.Email ?? "test@test.gmail";
            }

            return new ApiResult<ShowDonationsViewModel>(
                data,
                pageIndex,
                pageSize,
                count,
                sortColumn,
                sortOrder,
                filterColumn,
                filterQuery
                );
        }

        public DonationService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<bool> AddDonation(AddDonationViewModel viewModel)
        {
            var project = await _context.Projects.FindAsync(viewModel.ProjectId);

            var user = await _context.ApplicationUsers.FindAsync(viewModel.UserId);

            var donation = new Donation()
            {
                Email = viewModel.Email,
                Message = viewModel.Message,
                Project = project,
                User = user,
                Value = viewModel.Value
            };

            await _context.Donations.AddAsync(donation);
            var result = await _context.SaveChangesAsync();

            return result >= 0 ? true : false;
        }
    }
}
