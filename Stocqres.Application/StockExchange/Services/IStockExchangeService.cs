using System.Threading.Tasks;
using Stocqres.Application.Stock.Dto;

namespace Stocqres.Application.StockExchange.Services
{
    public interface IStockExchangeService
    {
        Task<decimal> GetStockPrice(string stockCode);
    }
}