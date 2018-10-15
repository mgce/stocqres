using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Stocqres.Core.Domain;

namespace Stocqres.Infrastructure.ProjectionReader
{
    public interface IProjectionReader
    {
        Task<T> GetAsync<T>(Expression<Func<T, bool>> predicate) where T : IProjection;
        Task<IEnumerable<T>> FindAsync<T>(Expression<Func<T, bool>> predicate) where T : IProjection;
    }
}