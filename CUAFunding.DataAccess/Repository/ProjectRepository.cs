using CUAFunding.DomainEntities.Entities;
using CUAFunding.Interfaces.Repository;
using CUAFunding.ViewModels;
using CUAFunding.ViewModels.ProjectViewModel;
using Microsoft.EntityFrameworkCore;
using System.Linq.Dynamic.Core;
using System;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using static CUAFunding.Common.Helpers.DynamicHelper;

namespace CUAFunding.DataAccess.Repository
{
    public class ProjectRepository : BaseRepository<Project>, IProjectRepository
    {
        Type entityType = typeof(Project);

        public ProjectRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<ApiResult<ShowProjectViewModel>> GetApiResult(
            int pageIndex,
            int pageSize,
            string sortColumn = null,
            string sortOrder = null,
            string filterColumn = null,
            string filterQuery = null)
        {
            var source = _context.Projects.Select(proj => new ShowProjectViewModel()
            {
                Id = proj.Id,
                OwnerId = proj.OwnerId,
                PageVisitorsCount = proj.PageVisitorsCount,
                AvgRating = proj.Marks.Average(mark => mark.Value),
                Сollected = proj.Donations.Sum(donation => donation.Value),
                Title = proj.Title,
                Goal = proj.Goal,
                LocationX = proj.Location.Coordinate.X,
                LocationY = proj.Location.Coordinate.Y,
                ExpirationDate = proj.ExpirationDate
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

            return new ApiResult<ShowProjectViewModel>(
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

        public async Task<decimal> GetCollected(string id)
        {
            var res = await _context.Donations.Where(item => item.ProjectId == id).SumAsync(item => item.Value);

            return res;
        }

        public async Task<double> GetRanting(string id)
        {
            var res = await _context.Marks.Where(item => item.ProjectId == id).SumAsync(item => item.Value);

            return res;
        }

        public async Task<ApiResult<ShowProjectViewModel>> GetApiResult(
            int pageIndex,
            int pageSize,
            string sortColumn = null,
            string sortOrder = null,
            string filterColumn = null,
            string filterQuery = null,
            string userId = null)
        {
            var source = _context.Projects.Select(proj => new ShowProjectViewModel()
            {
                Id = proj.Id,
                OwnerId = proj.OwnerId,
                PageVisitorsCount = proj.PageVisitorsCount,
                AvgRating = proj.Marks.Average(mark => mark.Value),
                Сollected = proj.Donations.Sum(donation => donation.Value),
                Title = proj.Title,
                Goal = proj.Goal,
                LocationX = proj.Location.Coordinate.X,
                LocationY = proj.Location.Coordinate.Y,
                ExpirationDate = proj.ExpirationDate
            });

            if (!String.IsNullOrEmpty(userId) && !String.IsNullOrEmpty(userId))
            {
                source = source.Where(item => item.OwnerId == userId);
            }

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

            return new ApiResult<ShowProjectViewModel>(
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
    }
}
