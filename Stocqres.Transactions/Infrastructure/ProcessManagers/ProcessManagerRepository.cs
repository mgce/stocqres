using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Stocqres.Core.Commands;
using Stocqres.Core.Dispatcher;

namespace Stocqres.Transactions.Infrastructure.ProcessManagers
{
    public interface IProcessManagerRepository<T> where T : Orders.Domain.ProcessManagers.ProcessManager
    {
        Task<T> Get(Guid aggregateId);
        Task Save(T processManager);
    }

    public class ProcessManagerRepository<T> : IProcessManagerRepository<T> where T:Orders.Domain.ProcessManagers.ProcessManager
    {
        protected readonly ProcessManagerDbContext _dbContext;
        private readonly IDispatcher _dispatcher;

        public ProcessManagerRepository(ProcessManagerDbContext dbContext, IDispatcher dispatcher)
        {
            _dbContext = dbContext;
            _dispatcher = dispatcher;
        }

        public async Task<T> Get(Guid aggregateId)
        {
            return await _dbContext.Set<T>().SingleOrDefaultAsync(pm => pm.AggregateId == aggregateId);

        }

        public async Task Save(T processManager)
        {
            processManager.UpdateModifiedDate();
            var pm = await Get(processManager.AggregateId);
            if (pm == null)
            {
                await _dbContext.Set<T>().AddAsync(processManager);
                await _dbContext.SaveChangesAsync();
                await HandleCommands(processManager);
            }
            else
            {
                _dbContext.Set<T>().Update(processManager);
                await _dbContext.SaveChangesAsync();
                await HandleCommands(pm);
            }
            
        }

        private async Task HandleCommands(Orders.Domain.ProcessManagers.ProcessManager processManager)
        {
            var commandsToHandle = new List<ICommand>(processManager.GetUnhandledCommands());
            
            foreach (var command in commandsToHandle)
            {
                processManager.TakeOffCommand(command);
                await _dispatcher.SendAsync(command);
            }
        }
    }
}
