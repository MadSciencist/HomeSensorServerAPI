using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HomeSensorServerAPI.Repository
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        protected readonly AppDbContext _context;
        private readonly ILogger _logger;

        public GenericRepository(AppDbContext context, ILoggerFactory loggerFactory)
        {
            _context = context;
            _logger = loggerFactory.CreateLogger("HomeSensorServerAPI.Repository.GenericRepository");
        }

        public virtual async Task<T> CreateAsync(T entity)
        {
            await _context.AddAsync(entity);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException e)
            {
                _logger.LogError(e, "Error while updading entity - concurrency");
            }
            catch (DbUpdateException e)
            {
                _logger.LogError(e, "Error while updading entity");
            }

            return entity;
        }

        public async Task DeleteAsync(T entity)
        {
            _context.Remove(entity);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException e)
            {
                _logger.LogError(e, "Error while updading entity - concurrency");
            }
            catch (DbUpdateException e)
            {
                _logger.LogError(e, "Error while updading entity");
            }
        }

        public IEnumerable<T> Find(Func<T, bool> predicate)
        {
            return _context.Set<T>().Where(predicate);
        }

        public IEnumerable<T> GetAll()
        {
            return _context.Set<T>();
        }

        public virtual async Task<T> GetByIdAsync(int id)
        {
            return await _context.Set<T>().FindAsync(id);
        }

        public virtual async Task<T> UpdateAsync(T entity)
        {
            _context.Entry(entity).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException e)
            {
                _logger.LogError(e, "Error while updading entity - concurrency");
            }
            catch (DbUpdateException e)
            {
                _logger.LogError(e, "Error while updading entity");
            }

            return entity;
        }
    }
}
