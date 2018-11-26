using System;
using System.Threading.Tasks;
using MongoDB.Driver;
using Stocqres.Core.Domain;
using Stocqres.Infrastructure.UnitOfWork;

namespace Stocqres.Infrastructure.Projections
{
    public class ProjectionWriter : IProjectionWriter
    {
        private readonly IMongoDatabase _database;

        public ProjectionWriter(IMongoDatabase database, IUnitOfWork unitOfWork)
        {
            _database = unitOfWork.MongoDatabase;
        }

        public async Task AddAsync<T>(T projection)
        {
            var collection = GetCollection<T>();

            await collection.InsertOneAsync(projection);
        }

        public async Task UpdateAsync<T>(Guid id, Action<T> action) where T: IProjection
        {
            var collection = GetCollection<T>();

            var view = await collection.Find(v => v.Id == id).SingleOrDefaultAsync();
            var typeView = view is T variable ? variable : default(T);

            action(typeView);
            await collection.ReplaceOneAsync(v => v.Id == id, view);
        }

        private IMongoCollection<T> GetCollection<T>() => _database.GetCollection<T>(typeof(T).Name);
    }
}
