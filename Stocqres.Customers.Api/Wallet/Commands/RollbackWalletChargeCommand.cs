using System;
using Stocqres.Core.Commands;

namespace Stocqres.Customers.Api.Wallet.Commands
{
    public class RollbackWalletChargeCommand : ICommand
    {
        public Guid WalletId { get; set; }
        public Guid OrderId { get; set; }
        public decimal AmountToRollback { get; set; }

        public RollbackWalletChargeCommand(Guid walletId, Guid orderId, decimal amountToRollback)
        {
            WalletId = walletId;
            OrderId = orderId;
            AmountToRollback = amountToRollback;
        }
    }
}
