using System.Data;
using MongoDB.Driver;

namespace Stocqres.Infrastructure.UnitOfWork
{
    public interface IUnitOfWork
    {
        IDbConnection Connection { get; }
        IDbTransaction Transaction { get; }
        IMongoDatabase MongoDatabase { get; }
        void BeginTransaction();
        void Commit();
        void Rollback();
    }
}