using System;
using System.Collections.Generic;
using System.Text;
using Stocqres.Core.Commands;

namespace Stocqres.SharedKernel.Commands
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
