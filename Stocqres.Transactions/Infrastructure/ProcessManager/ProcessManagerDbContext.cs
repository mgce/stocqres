using Microsoft.EntityFrameworkCore;
using Stocqres.Transactions.Orders.Domain;
using Stocqres.Transactions.Orders.Domain.OrderProcessManager;

namespace Stocqres.Transactions.Infrastructure.ProcessManager
{
    public class ProcessManagerDbContext : DbContext
    {
        public ProcessManagerDbContext(DbContextOptions<ProcessManagerDbContext> options) : base(options)
        {
        }

        public DbSet<OrderProcessManager> OrderProcessManagers { get; set; }
    }
}
