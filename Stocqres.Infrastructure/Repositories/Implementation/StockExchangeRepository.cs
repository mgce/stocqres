using MongoDB.Driver;
using Stocqres.Domain;
using Stocqres.Infrastructure.Repositories.Api;

namespace Stocqres.Infrastructure.Repositories.Implementation
{
    public class StockExchangeRepository : Repository<StockExchange>, IStockExchangeRepository
    {
        public StockExchangeRepository(IMongoDatabase database) : base(database)
        {
        }
    }
}
