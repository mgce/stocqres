using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Driver;
using Stocqres.Domain;

namespace Stocqres.Infrastructure
{
    public class WalletRepository : Repository<Wallet>, IWalletRepository
    {
        public WalletRepository(IMongoDatabase database) : base(database)
        {
        }
    }
}
