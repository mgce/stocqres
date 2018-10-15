using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Driver;
using Stocqres.Core.Domain;

namespace Stocqres.Infrastructure.ProjectionReader
{
    public class ProjectionReader : IProjectionReader
    {
        private readonly IMongoDatabase _database;

        public ProjectionReader(IMongoDatabase database)
        {
            _database = database;
        }

        public async Task<T> GetAsync<T>(Expression<Func<T, bool>> predicate) where T : IProjection
        {
            var collection = GetCollection<T>();
            return await collection.Find(predicate).SingleOrDefaultAsync();
        }

        public async Task<IEnumerable<T>> FindAsync<T>(Expression<Func<T, bool>> predicate) where T : IProjection
        {
            var collection = GetCollection<T>();
            return await collection.Find(predicate).ToListAsync();
        }

        private IMongoCollection<T> GetCollection<T>() => _database.GetCollection<T>(typeof(T).Name);
    }
}
