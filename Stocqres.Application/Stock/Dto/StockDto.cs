using System;
using System.Collections.Generic;
using System.Text;

namespace Stocqres.Application.Stock.Dto
{
    public class StockDto
    {
        public string Name { get; set; }
        public string Code { get; set; }
        public int Unit { get; set; }
        public decimal Price { get; set; }
    }
}
