using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Stocqres.Core.EventStore;
using Stocqres.Domain.Enums;

namespace Stocqres.Application.StockGroup.Services
{
    public class StockGroupService : IStockGroupService
    {
        private readonly ICustomEventStore _eventStore;

        public StockGroupService(ICustomEventStore eventStore)
        {
            _eventStore = eventStore;
        }

        public async Task<Domain.StockGroup> GetStockGroupFromStockExchange(Guid stockExchangeId, Guid stockId)
        {
            return await _eventStore.GetAsync<Domain.StockGroup>(x =>
                x.OwnerId == stockExchangeId &&
                x.StockOwner == StockOwner.StockExchange &&
                x.StockId == stockId);
        }

        public async Task<Domain.StockGroup> GetUserStockGroup(Guid userId, Guid stockId)
        {
            return await _eventStore.GetAsync<Domain.StockGroup>(x =>
                x.OwnerId == userId &&
                x.StockOwner == StockOwner.User &&
                x.StockId == stockId);
        }
    }
}
