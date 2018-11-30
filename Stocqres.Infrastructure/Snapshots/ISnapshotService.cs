using System;
using System.Threading.Tasks;
using Stocqres.Core.Domain;
using Stocqres.Core.EventSourcing;

namespace Stocqres.Infrastructure.Snapshots
{
    public interface ISnapshotService
    {
        Task TakeAndSaveAsync(IAggregateRoot aggregate);
        Task<Snapshot> GetLastAsync(Guid aggregateId);
    }
}