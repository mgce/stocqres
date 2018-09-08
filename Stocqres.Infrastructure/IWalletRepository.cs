using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Stocqres.Domain;

namespace Stocqres.Infrastructure
{
    public interface IWalletRepository : IRepository<Wallet>
    {
    }
}
