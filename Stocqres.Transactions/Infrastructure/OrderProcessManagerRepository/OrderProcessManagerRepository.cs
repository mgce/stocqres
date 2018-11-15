using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Stocqres.Core.Dispatcher;
using Stocqres.Transactions.Infrastructure.ProcessManager;
using Stocqres.Transactions.Infrastructure.ProcessManagerRepository;
using Stocqres.Transactions.Orders.Domain.OrderProcessManager;

namespace Stocqres.Transactions.Infrastructure.OrderProcessManagerRepository
{
    public interface IOrderProcessManagerRepository : IProcessManagerRepository<OrderProcessManager>
    {
        Task<List<OrderProcessManager>> GetAsync(Expression<Func<OrderProcessManager, bool>> predicate);
        Task<OrderProcessManager> FindAsync(Expression<Func<OrderProcessManager, bool>> predicate);
    }

    public class OrderProcessManagerRepository : ProcessManagerRepository<OrderProcessManager>, IOrderProcessManagerRepository
    {
        public OrderProcessManagerRepository(ProcessManagerDbContext dbContext, IDispatcher dispatcher) : base(dbContext, dispatcher)
        {
        }

        public async Task<List<OrderProcessManager>> GetAsync(Expression<Func<OrderProcessManager, bool>> predicate)
        {
            return await _dbContext.OrderProcessManagers.Where(predicate).ToListAsync();
        }

        public async Task<OrderProcessManager> FindAsync(Expression<Func<OrderProcessManager, bool>> predicate)
        {
            return await _dbContext.OrderProcessManagers.SingleOrDefaultAsync(predicate);
        }
    }
}
