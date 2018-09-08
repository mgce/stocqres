using MongoDB.Driver;
using Stocqres.Domain;
using Stocqres.Infrastructure.Repositories.Api;

namespace Stocqres.Infrastructure.Repositories.Implementation
{
    public class WalletRepository : Repository<Wallet>, IWalletRepository
    {
        public WalletRepository(IMongoDatabase database) : base(database)
        {
        }
    }
}
