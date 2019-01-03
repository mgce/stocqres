using System;
using Stocqres.Core.Commands;

namespace Stocqres.Customers.Api.Companies.Commands
{
    public class AddStocksToCompanyCommand : ICommand
    {
        public Guid CompanyId { get; set; }
        public Guid OrderId { get; set; }
        public int Quantity { get; set; }
        public string StockCode { get; set; }

        public AddStocksToCompanyCommand(Guid companyId, Guid orderId, int quantity, string stockCode)
        {
            CompanyId = companyId;
            OrderId = orderId;
            Quantity = quantity;
            StockCode = stockCode;
        }
    }
}
