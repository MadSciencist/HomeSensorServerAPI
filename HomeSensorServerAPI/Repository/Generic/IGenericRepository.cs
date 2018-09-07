using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HomeSensorServerAPI.Repository.Generic
{
    public interface IGenericRepository<T> where T : class
    {
        IQueryable<T> GetAll();
        IEnumerable<T> Find(Func<T, bool> predicate);
        Task<T> GetByIdAsync(int id);
        Task<T> CreateAsync(T entity);
        Task<T> UpdateAsync(T entity);
        Task DeleteAsync(T entity);
    }
}
