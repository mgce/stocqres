using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using MediatR;

namespace Stocqres.Core.Commands
{
    public class CommandBus : ICommandBus
    {
        private readonly Mediator _mediator;

        public CommandBus(Mediator mediator)
        {
            _mediator = mediator;
        }

        public Task Send<TCommand>(TCommand command) where TCommand : ICommand
        {
            return _mediator.Send(command);
        }
    }
}
