using System;
using System.Threading.Tasks;

namespace Stocqres.Application.StockGroup.Services
{
    public interface IStockGroupService
    {
        Task<Domain.StockGroup> GetStockGroupFromStockExchange(Guid stockExchangeId, Guid stockId);
        Task<Domain.StockGroup> GetUserStockGroup(Guid userId, Guid stockId);
    }
}