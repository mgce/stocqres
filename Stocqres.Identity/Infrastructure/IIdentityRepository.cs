using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Stocqres.Core;

namespace Stocqres.Identity.Infrastructure
{
    public interface IIdentityRepository<T> where T : BaseEntity
    {
        Task<T> FindByIdAsync(Guid id);
        Task<T> FindAsync(Expression<Func<T, bool>> expression);
        Task<IEnumerable<T>> FindAllAsync();
        Task<IEnumerable<T>> FindByConditionAync(Expression<Func<T, bool>> expression);
        Task CreateAsync(T entity);
        void Update(T entity);
        void Delete(T entity);
        Task SaveAsync();
    }
}