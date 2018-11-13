using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Stocqres.Core.Commands;
using Stocqres.Core.Dispatcher;
using Stocqres.Core.Events;

namespace Stocqres.Transactions.Infrastructure.ProcessManager
{
    public interface IProcessManagerRepository
    {
        Task<T> Get<T>(Guid aggregateId) where T : Orders.Domain.ProcessManager;
        Task Save<T>(T processManager) where T : Orders.Domain.ProcessManager;
    }

    public class ProcessManagerRepository : IProcessManagerRepository
    {
        private readonly ProcessManagerDbContext _dbContext;
        private readonly IDispatcher _dispatcher;

        public ProcessManagerRepository(ProcessManagerDbContext dbContext, IDispatcher dispatcher)
        {
            _dbContext = dbContext;
            _dispatcher = dispatcher;
        }

        public async Task<T> Get<T>(Guid aggregateId) where T : Orders.Domain.ProcessManager
        {
            return await _dbContext.Set<T>().SingleOrDefaultAsync(pm => pm.AggregateId == aggregateId);

        }

        public async Task Save<T>(T processManager) where T : Orders.Domain.ProcessManager
        {
            processManager.UpdateModifiedDate();
            var pm = await Get<T>(processManager.AggregateId);
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

        private async Task HandleCommands(Orders.Domain.ProcessManager processManager)
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
