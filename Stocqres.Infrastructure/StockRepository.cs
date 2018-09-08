using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Driver;
using Stocqres.Domain;

namespace Stocqres.Infrastructure
{
    public class StockRepository : Repository<Stock>, IStockRepository
    {
        public StockRepository(IMongoDatabase database) : base(database)
        {
        }
    }
}
