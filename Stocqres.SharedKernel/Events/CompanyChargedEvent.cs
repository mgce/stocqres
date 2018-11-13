using System;
using Stocqres.Core.Events;

namespace Stocqres.SharedKernel.Events
{
    public class CompanyChargedEvent : IEvent
    {
        public Guid AggregateId { get; set; }
        public string StockName { get; set; }
        public string StockCode { get; set; }
        public int StockUnit { get; set; }
        public int StockQuantity { get; set; }

        public CompanyChargedEvent(Guid aggregateId, string stockName, string stockCode, int stockUnit, int stockQuantity)
        {
            AggregateId = aggregateId;
            StockName = stockName;
            StockCode = stockCode;
            StockUnit = stockUnit;
            StockQuantity = stockQuantity;
        }
    }
}
