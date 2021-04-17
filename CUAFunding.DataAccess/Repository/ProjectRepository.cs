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

namespace CUAFunding.DataAccess.Repository
{
    public class ProjectRepository : BaseRepository<Project>, IProjectRepository
    {
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
                ImagePath = proj.ImagePath,
                OwnerId = proj.OwnerId,
                PageVisitorsCount = proj.PageVisitorsCount,
                AvgRating = proj.Marks.Average(mark => mark.Value),
                Сollected = proj.Donations.Sum(donation => donation.Value),
                Title = proj.Title,
                Goal = proj.Goal
            });

            if (!String.IsNullOrEmpty(filterColumn) && !String.IsNullOrEmpty(filterQuery) && IsValidProperty(filterColumn))
            {
                source = source.Where(String.Format("{0}.Contains(@0)", filterColumn), filterQuery);
            }

            if (!String.IsNullOrEmpty(sortColumn) && IsValidProperty(sortColumn))
            {
                sortOrder = !String.IsNullOrEmpty(sortOrder) && sortOrder.ToUpper() == "ASC" ? "ASC" : "DESC";
                source = source.OrderBy(String.Format("{0} {1}"), sortColumn, sortOrder);
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

        public static bool IsValidProperty(string propertyName, bool throwExceptionIfNotFound = true)
        {
            var prop = typeof(Project).GetProperty(
                propertyName,
                BindingFlags.IgnoreCase |
                BindingFlags.Public |
                BindingFlags.Static |
                BindingFlags.Instance);

            if (prop == null && throwExceptionIfNotFound)
            {
                throw new NotSupportedException($"Property {propertyName} doesn`t exist.");
            }
            var isValid = prop != null;

            return isValid;
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
    }
}
