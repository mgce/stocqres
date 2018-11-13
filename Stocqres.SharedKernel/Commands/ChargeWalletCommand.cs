using System;
using System.Collections.Generic;
using System.Text;
using Stocqres.Core.Commands;

namespace Stocqres.SharedKernel.Commands
{
    public class ChargeWalletCommand : ICommand
    {
        public Guid WalletId { get; set; }
        public Guid CompanyId { get; set; }
        public int Quantity { get; set; }

        public ChargeWalletCommand(Guid walletId, Guid companyId, int quantity)
        {
            WalletId = walletId;
            CompanyId = companyId;
            Quantity = quantity;
        }
    }
}
