using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Stocqres.Infrastructure.ExternalServices.StockExchangeService
{
    public class StockExchangeService : IStockExchangeService
    {
        private readonly HttpClient _httpClient;

        public StockExchangeService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<decimal> GetStockPrice(string stockCode)
        {
            var response = await _httpClient.GetAsync("");
            var responseContent = await response.Content.ReadAsStringAsync();
            var stocks = JsonConvert.DeserializeObject<StocksDto>(responseContent);

            return stocks.Items.Single(x => x.Code == stockCode).Price;
        }
    }
}
