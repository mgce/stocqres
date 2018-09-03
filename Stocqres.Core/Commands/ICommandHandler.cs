using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using MediatR;

namespace Stocqres.Core.Commands
{
    public interface ICommandHandler<in TCommand> : IRequestHandler<TCommand> 
        where TCommand : ICommand
    {
    }
}
