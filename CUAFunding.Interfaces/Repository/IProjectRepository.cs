using CUAFunding.DomainEntities.Entities;
using CUAFunding.ViewModels;
using CUAFunding.ViewModels.ProjectViewModel;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CUAFunding.Interfaces.Repository
{
    public interface IProjectRepository : IBaseRepository<Project>
    {
        Task<double> GetRanting(Guid Id);
        Task<decimal> GetCollected(Guid Id);

        Task<ApiResult<ShowProjectViewModel>> GetApiResult(
            int pageIndex,
            int pageSize,
            string sortColumn,
            string sortOrder,
            string filterColumn,
            string filterQuery);
    }
}
