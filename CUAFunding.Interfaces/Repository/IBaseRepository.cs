using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CUAFunding.Interfaces.Repository
{
    public interface IBaseRepository<TEntity> where TEntity : class
    {
        Task Delete(TEntity entity);
        Task Delete(string id);
        Task<TEntity> Find(string id);
        Task Insert(TEntity entity);
        Task Update(TEntity entity);
    }
}
