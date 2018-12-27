using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Autofac;

namespace Stocqres.Core.Queries
{
    public class QueryBus : IQueryBus
    {
        private readonly IComponentContext _context;

        public QueryBus(IComponentContext context)
        {
            _context = context;
        }

        public async Task<TResult> QueryAsync<TQuery, TResult>(TQuery query) where TQuery : IQuery<TResult>
        {
            var handlerType = typeof(IQueryHandler<,>)
                .MakeGenericType(query.GetType(), typeof(TResult));

            dynamic handler = _context.Resolve(handlerType);

            return await handler.HandleAsync((dynamic)query);
        }
    }
}
