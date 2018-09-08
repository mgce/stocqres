using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using MongoDB.Driver;
using Stocqres.Core;
using Stocqres.Infrastructure.Repositories.Api;

namespace Stocqres.Infrastructure.Repositories.Implementation
{
    public class Repository<T> : IRepository<T> where T : BaseEntity
    {
        protected IMongoCollection<T> Collection { get; }

        public Repository(IMongoDatabase database)
        {
            var collectionName = typeof(T).Name;
            Collection = database.GetCollection<T>(collectionName);
        }

        public async Task<T> GetAsync(Expression<Func<T, bool>> predicate)
        {
            return await Collection.Find(predicate).SingleOrDefaultAsync();
        }

        public async Task<T> GetAsync(Guid id)
        {
            return await GetAsync(x => x.Id == id);
        }

        public async Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate)
        {
            return await Collection.Find(predicate).ToListAsync();
        }

        public async Task CreateAsync(T entity)
            => await Collection.InsertOneAsync(entity);

        public async Task UpdateAsync(T entity)
            => await Collection.ReplaceOneAsync(e => e.Id == entity.Id, entity);

        public async Task UpdateAsync(params T[] entities)
        {
            foreach (var entity in entities)
            {
                await Collection.ReplaceOneAsync(e => e.Id == entity.Id, entity);
            }
        }

        public async Task DeleteAsync(Guid id)
            => await Collection.DeleteOneAsync(e => e.Id == id);

        public async Task<bool> IsExist(Guid id)
        {
            return await Collection.FindAsync(x => x.Id == id) != null;
        }
    }
}
