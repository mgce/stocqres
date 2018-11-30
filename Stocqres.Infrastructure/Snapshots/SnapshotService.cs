using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Stocqres.Core.Domain;
using Stocqres.Core.EventSourcing;
using Stocqres.Infrastructure.DatabaseProvider;
using Stocqres.Infrastructure.EventRepository;
using Stocqres.Infrastructure.EventRepository.Scripts;

namespace Stocqres.Infrastructure.Snapshots
{
    public class SnapshotService : ISnapshotService
    {
        private readonly IDatabaseProvider _databaseProvider;

        public SnapshotService(IDatabaseProvider databaseProvider)
        {
            _databaseProvider = databaseProvider;
        }

        public async Task TakeAndSaveAsync(IAggregateRoot aggregate)
        {
            var snapshot = aggregate.ToSnapshot();

            string insertScript = EventRepositoryScriptsAsStrings.InsertSnapshot();

            await _databaseProvider.ExecuteAsync(insertScript, snapshot);
        }

        public async Task<Snapshot> GetLastAsync(Guid aggregateId)
        {
            var sql = EventRepositoryScriptsAsStrings.GetAggregateSnapshot(aggregateId);
            return await _databaseProvider.QueryFirstOrDefaultAsync<Snapshot>(sql);
        }
    }
}
