using System;
using System.Collections.Generic;
using System.Text;
using Stocqres.Core.Commands;

namespace Stocqres.SharedKernel.Commands
{
    public class AddStockToWalletCommand : ICommand
    {
        public Guid WalletId { get; set; }
        public string StockName { get; set; }
        public string StockCode { get; set; }
        public int StockUnit { get; set; }
        public int StockQuantity { get; set; }

        public AddStockToWalletCommand(Guid walletId, string stockName, string stockCode, int stockUnit, int stockQuantity)
        {
            WalletId = walletId;
            StockName = stockName;
            StockCode = stockCode;
            StockUnit = stockUnit;
            StockQuantity = stockQuantity;
        }
    }
}
