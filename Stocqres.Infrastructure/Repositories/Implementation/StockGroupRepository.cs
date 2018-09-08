using MongoDB.Driver;
using Stocqres.Domain;
using Stocqres.Infrastructure.Repositories.Api;

namespace Stocqres.Infrastructure.Repositories.Implementation
{
    public class StockGroupRepository : Repository<StockGroup>, IStockGroupRepository
    {
        public StockGroupRepository(IMongoDatabase database) : base(database)
        {
        }
    }
}
