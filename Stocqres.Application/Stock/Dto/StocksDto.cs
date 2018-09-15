using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace Stocqres.Application.Stock.Dto
{
    public class StocksDto
    {
        [JsonProperty("publicationDate")]
        public DateTime PublicationDate { get; set; }
        [JsonProperty("items")]
        public StockDto[] Items { get; set; }
    }
}
