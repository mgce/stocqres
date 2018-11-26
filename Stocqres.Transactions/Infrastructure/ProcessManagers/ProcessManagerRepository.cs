using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using Dapper;
using Microsoft.EntityFrameworkCore;
using Stocqres.Core.Commands;
using Stocqres.Core.Dispatcher;
using Stocqres.Infrastructure.UnitOfWork;
using Stocqres.Transactions.Orders.Domain.ProcessManagers;

namespace Stocqres.Transactions.Infrastructure.ProcessManagers
{
    public interface IProcessManagerRepository<T> where T : ProcessManager
    {
        Task<T> Get(Guid aggregateId);
        Task Save(T processManager);
    }

    public class ProcessManagerRepository<T> : IProcessManagerRepository<T> where T: ProcessManager
    {
        protected ProcessManagerDbContext _dbContext;
        private readonly IDispatcher _dispatcher;
        private readonly IUnitOfWork _unitOfWork;
        protected IDbTransaction _transaction => _unitOfWork.Transaction;
        protected IDbConnection _connection => _unitOfWork.Connection;

        public ProcessManagerRepository(ProcessManagerDbContext dbContext, IDispatcher dispatcher, IUnitOfWork unitOfWork)
        {
            _dbContext = dbContext;
            _dispatcher = dispatcher;
            _unitOfWork = unitOfWork;
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

        protected async Task HandleCommands(ProcessManager processManager)
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
