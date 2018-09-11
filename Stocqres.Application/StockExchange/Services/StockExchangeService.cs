using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Stocqres.Application.Stock.Dto;

namespace Stocqres.Application.StockExchange.Services
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
            var stocks = JsonConvert.DeserializeObject<List<StockDto>>(responseContent);

            return stocks.Single(s => s.Code == stockCode).Price;
        }
    }
}
