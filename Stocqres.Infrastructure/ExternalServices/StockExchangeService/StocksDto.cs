using System;
using Newtonsoft.Json;

namespace Stocqres.Infrastructure.ExternalServices.StockExchangeService
{
    public class StocksDto
    {
        [JsonProperty("publicationDate")]
        public DateTime PublicationDate { get; set; }
        [JsonProperty("items")]
        public StockDto[] Items { get; set; }
    }
}
