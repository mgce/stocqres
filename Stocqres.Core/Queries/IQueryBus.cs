using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Stocqres.Core.Queries
{
    public interface IQueryBus
    {
        Task<TResponse> QueryAsync<TQuery, TResponse>(TQuery query) where TQuery : IQuery<TResponse>;
    }
}
