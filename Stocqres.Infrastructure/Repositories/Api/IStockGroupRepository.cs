using System;
using System.Threading.Tasks;
using Stocqres.Domain;

namespace Stocqres.Infrastructure.Repositories.Api
{
    public interface IStockGroupRepository : IRepository<StockGroup>
    {
        Task<StockGroup> GetStockGroupFromStockExchange(Guid stockExchangeId, Guid stockId);
        Task<StockGroup> GetUserStockGroup(Guid userId, Guid stockId);
    }
}
