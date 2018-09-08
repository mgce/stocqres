using System;
using Stocqres.Core;

namespace Stocqres.Domain
{
    public class Stock : BaseEntity
    {
        public Guid StockExchangeId { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public int Unit { get; set; }
        public decimal Price { get; set; }
    }
}
