using System;
using System.Collections.Generic;
using System.Text;
using Stocqres.Core.Commands;

namespace Stocqres.SharedKernel.Commands
{
    public class TopUpWalletAmountCommand : ICommand
    {
        public Guid WalletId { get; set; }
        public Guid OrderId { get; set; }
        public string StockCode { get; set; }
        public int Quantity { get; set; }

        public TopUpWalletAmountCommand(Guid walletId, Guid orderId, string stockCode, int quantity)
        {
            WalletId = walletId;
            OrderId = orderId;
            StockCode = stockCode;
            Quantity = quantity;
        }
    }
}
