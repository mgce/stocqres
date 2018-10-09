using System.Collections.Generic;
using Stocqres.Core;
using Stocqres.Core.Domain;

namespace Stocqres.Domain
{
    public class StockExchange : AggregateRoot
    {
        public List<StockGroup> StockGroups { get; set; }
    }
}
