using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace Stocqres.Infrastructure.DatabaseProvider
{
    public interface IDatabaseProvider
    {
        Task<IEnumerable<T>> QueryAsync<T>(string sql, object parameters = null);
        Task<int> ExecuteAsync(string sql, object parameters = null);
        Task<object> ExecuteScalarAsync(string sql, object parameters= null);
        Task<T> QueryFirstOrDefaultAsync<T>(string sql, object parameters = null)
    }
}