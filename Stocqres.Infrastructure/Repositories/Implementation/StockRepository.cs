using MongoDB.Driver;
using Stocqres.Domain;
using Stocqres.Infrastructure.Repositories.Api;

namespace Stocqres.Infrastructure.Repositories.Implementation
{
    public class StockRepository : Repository<Stock>, IStockRepository
    {
        public StockRepository(IMongoDatabase database) : base(database)
        {
        }
    }
}
