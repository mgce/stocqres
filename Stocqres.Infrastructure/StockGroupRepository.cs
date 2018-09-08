
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Driver;
using Stocqres.Domain;

namespace Stocqres.Infrastructure
{
    public class StockGroupRepository : Repository<StockGroup>, IStockGroupRepository
    {
        public StockGroupRepository(IMongoDatabase database) : base(database)
        {
        }
    }
}
