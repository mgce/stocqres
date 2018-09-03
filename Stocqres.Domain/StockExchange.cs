using System.Collections.Generic;
using Stocqres.Core;

namespace Stocqres.Domain
{
    public class StockExchange : AggregateRoot
    {
        public List<StockGroup> StockGroups { get; set; }
    }
}
