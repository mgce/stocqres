using System.Threading.Tasks;
using Autofac;

namespace Stocqres.Core.Commands
{
    public class CommandBus : ICommandBus
    {
        private readonly IComponentContext _context;

        public CommandBus(IComponentContext context)
        {
            _context = context;
        }

        public async Task SendAsync<TCommand>(TCommand command) where TCommand : ICommand 
            => await _context.Resolve<ICommandHandler<TCommand>>().HandleAsync(command);
    }
}
