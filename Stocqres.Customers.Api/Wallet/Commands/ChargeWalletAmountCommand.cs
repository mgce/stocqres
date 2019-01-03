using System;
using Stocqres.Core.Commands;

namespace Stocqres.Customers.Api.Wallet.Commands
{
    public class ChargeWalletAmountCommand : ICommand
    {
        public Guid WalletId { get; set; }
        public Guid CompanyId { get; set; }
        public Guid OrderId { get; set; }
        public int Quantity { get; set; }

        public ChargeWalletAmountCommand(Guid walletId, Guid companyId,Guid orderId, int quantity)
        {
            WalletId = walletId;
            CompanyId = companyId;
            OrderId = orderId;
            Quantity = quantity;
        }
    }
}
