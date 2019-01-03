using System;
using Stocqres.Core.Commands;

namespace Stocqres.Customers.Api.Wallet.Commands
{
    public class AddStockToWalletCommand : ICommand
    {
        public Guid WalletId { get; set; }
        public Guid OrderId { get; set; }
        public Guid CompanyId { get; set; }
        public string StockName { get; set; }
        public string StockCode { get; set; }
        public int StockUnit { get; set; }
        public int StockQuantity { get; set; }

        public AddStockToWalletCommand(Guid walletId, Guid orderId, Guid companyId, string stockName, string stockCode, int stockUnit, int stockQuantity)
        {
            WalletId = walletId;
            OrderId = orderId;
            CompanyId = companyId;
            StockName = stockName;
            StockCode = stockCode;
            StockUnit = stockUnit;
            StockQuantity = stockQuantity;
        }
    }
}
