using System;
using System.Collections.Generic;
using System.Text;
using Stocqres.Core.Commands;

namespace Stocqres.SharedKernel.Commands
{
    public class RollbackWalletChargeCommand : ICommand
    {
        public Guid WalletId { get; set; }
        public decimal AmountToRollback { get; set; }

        public RollbackWalletChargeCommand(Guid walletId, decimal amountToRollback)
        {
            WalletId = walletId;
            AmountToRollback = amountToRollback;
        }
    }
}
