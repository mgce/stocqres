using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Stocqres.Core;

namespace Stocqres.Identity.Infrastructure
{
    public class IdentityRepository<T> : IIdentityRepository<T> where T : BaseEntity
    {
        protected readonly IdentityDbContext _dbContext;

        public IdentityRepository(IdentityDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<T> FindByIdAsync(Guid id)
        {
            return await _dbContext.Set<T>().SingleOrDefaultAsync(x=>x.Id == id);
        }

        public async Task<T> FindAsync(Expression<Func<T, bool>> expression)
        {
            return await _dbContext.Set<T>().SingleOrDefaultAsync(expression);
        }

        public async Task<IEnumerable<T>> FindAllAsync()
        {
            return await _dbContext.Set<T>().ToListAsync();
        }

        public async Task<IEnumerable<T>> FindByConditionAync(Expression<Func<T, bool>> expression)
        {
            return await _dbContext.Set<T>().Where(expression).ToListAsync();
        }

        public async Task CreateAsync(T entity)
        {
            await _dbContext.Set<T>().AddAsync(entity);
        }

        public void Update(T entity)
        {
            _dbContext.Set<T>().Update(entity);
        }

        public void Delete(T entity)
        {
            _dbContext.Set<T>().Remove(entity);
        }

        public async Task SaveAsync()
        {
            await _dbContext.SaveChangesAsync();
        }
    }
}
