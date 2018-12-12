using Microsoft.EntityFrameworkCore;
using Stocqres.Transactions.Orders.Domain.ProcessManagers;

namespace Stocqres.Transactions.Infrastructure.ProcessManagers
{
    public class ProcessManagerDbContext : DbContext
    {
        public ProcessManagerDbContext(DbContextOptions<ProcessManagerDbContext> options) : base(options)
        {
        }

        public DbSet<BuyOrderProcessManager> OrderProcessManagers { get; set; }
    }
}
