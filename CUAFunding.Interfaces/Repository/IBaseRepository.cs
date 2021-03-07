using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CUAFunding.Interfaces.Repository
{
    public interface IBaseRepository<TEntity> where TEntity : class
    {
        Task Delete(TEntity entity);
        Task Delete(Guid id);
        Task<TEntity> Find(Guid id);
        Task Insert(TEntity entity);
        Task Update(TEntity entity);
    }
}
