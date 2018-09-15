using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace Stocqres.Application.Stock.Dto
{
    public class StockDto
    {
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("code")]
        public string Code { get; set; }
        [JsonProperty("unit")]
        public int Unit { get; set; }
        [JsonProperty("price")]
        public decimal Price { get; set; }
    }
}
