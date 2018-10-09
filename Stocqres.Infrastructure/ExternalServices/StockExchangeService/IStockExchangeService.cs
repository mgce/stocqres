using System.Threading.Tasks;

namespace Stocqres.Infrastructure.ExternalServices.StockExchangeService
{
    public interface IStockExchangeService
    {
        Task<decimal> GetStockPrice(string stockCode);
    }
}