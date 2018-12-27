using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Stocqres.Core.Commands;
using Stocqres.Core.Queries;

namespace Stocqres.Core.Dispatcher
{
    public class Dispatcher : IDispatcher
    {
        private readonly ICommandBus _commandBus;
        private readonly IQueryBus _queryBus;

        public Dispatcher(ICommandBus commandBus, IQueryBus queryBus)
        {
            _commandBus = commandBus;
            _queryBus = queryBus;
        }

        public async Task SendAsync<TCommand>(TCommand command) where TCommand : ICommand 
            => await _commandBus.SendAsync(command);

        public async Task<TResult> QueryAsync<TQuery, TResult>(TQuery query) where TQuery : IQuery<TResult> 
            => await _queryBus.QueryAsync<TQuery, TResult>(query);
    }
}
