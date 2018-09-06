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

        public async Task<TResult> Send<TQuery, TResult>(TQuery query) where TQuery : IQuery<TResult> 
            => await _context.Resolve<IQueryHandler<TQuery, TResult>>().HandleAsync(query);
    }
}
