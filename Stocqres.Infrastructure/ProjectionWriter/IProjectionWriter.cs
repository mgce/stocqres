using System;
using System.Threading.Tasks;
using Stocqres.Core.Domain;

namespace Stocqres.Infrastructure.ProjectionWriter
{
    public interface IProjectionWriter
    {
        Task AddAsync<T>(T projection);
        Task UpdateAsync<T>(Guid id, Action<T> action) where T : IProjection;
    }
}
