using System;
using System.Collections.Generic;
using System.Text;
using Stocqres.Core.Commands;

namespace Stocqres.Domain.Commands.Wallet
{
    public class CreateWalletCommand : ICommand
    {
        public Guid UserId { get; set; }
        public decimal Amount { get; set; }
    }
}
