using System;
using System.Threading.Tasks;
using MongoDB.Driver;
using Stocqres.Domain;
using Stocqres.Domain.Enums;
using Stocqres.Infrastructure.Repositories.Api;

namespace Stocqres.Infrastructure.Repositories.Implementation
{
    public class StockGroupRepository : Repository<StockGroup>, IStockGroupRepository
    {
        public StockGroupRepository(IMongoDatabase database) : base(database)
        {
        }

        public async Task<StockGroup> GetStockGroupFromStockExchange(Guid stockExchangeId, Guid stockId)
        {
            return await Collection.Find(x =>
                x.OwnerId == stockExchangeId &&
                x.StockOwner == StockOwner.StockExchange &&
                x.StockId == stockId).SingleOrDefaultAsync();
        }

        public async Task<StockGroup> GetUserStockGroup(Guid userId, Guid stockId)
        {
            return await Collection.Find(x =>
                x.OwnerId == userId &&
                x.StockOwner == StockOwner.User &&
                x.StockId == stockId).SingleOrDefaultAsync();
        }
    }
}
