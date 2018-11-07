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
        {
            ICommandHandler<TCommand> instance;
            _context.TryResolve(out instance);
            if(instance != null)
                await instance.HandleAsync(command);
            else
            {
                var handlerType = typeof(ICommandHandler<>).MakeGenericType(command.GetType());
                var commandHandlers = _context.Resolve(handlerType);
                var handlerMethod = commandHandlers.GetType().GetMethod("HandleAsync", new[] { command.GetType() });
                await (Task)((dynamic)handlerMethod.Invoke(commandHandlers, new object[] { command }));
            }
            //await _context.Resolve<ICommandHandler<TCommand>>().HandleAsync(command);
        }
            
    }
}
