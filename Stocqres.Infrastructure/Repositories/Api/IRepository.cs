using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Stocqres.Core;

namespace Stocqres.Infrastructure.Repositories.Api
{
    public interface IRepository<T> where T : BaseEntity
    {
        Task<T> GetAsync(Expression<Func<T, bool>> predicate);
        Task<T> GetByIdAsync(Guid id);
        Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate);
        Task CreateAsync(T entity);
        Task UpdateAsync(T entity);
        Task UpdateAsync(params T[] entities);
        Task DeleteAsync(Guid id);
        Task<bool> IsExist(Guid id);
    }
}
