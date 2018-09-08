using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Driver;
using Stocqres.Domain;

namespace Stocqres.Infrastructure
{
    public class StockExchangeRepository : Repository<StockExchange>, IStockExchangeRepository
    {
        public StockExchangeRepository(IMongoDatabase database) : base(database)
        {
        }
    }
}
