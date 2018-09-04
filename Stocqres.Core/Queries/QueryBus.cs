using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using MediatR;

namespace Stocqres.Core.Queries
{
    public class QueryBus : IQueryBus
    {
        private readonly Mediator _mediator;

        public QueryBus(Mediator mediator)
        {
            _mediator = mediator;
        }

        public Task<TResponse> Send<TQuery, TResponse>(TQuery query) where TQuery : IQuery<TResponse>
        {
            return _mediator.Send(query);
        }
    }
}
