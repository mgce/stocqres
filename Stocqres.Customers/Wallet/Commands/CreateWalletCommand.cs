using System;
using Stocqres.Core.Commands;

namespace Stocqres.Customers.Wallet.Commands
{
    public class CreateWalletCommand : ICommand
    {
        public Guid InvestorId { get; set; }
        public decimal Amount { get; set; }
    }
}
