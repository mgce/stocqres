using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Dapper;
using DapperExtensions;
using Microsoft.EntityFrameworkCore;
using Stocqres.Core.Commands;
using Stocqres.Core.Dispatcher;
using Stocqres.Infrastructure.UnitOfWork;
using Stocqres.Transactions.Orders.Domain.ProcessManagers;

namespace Stocqres.Transactions.Infrastructure.ProcessManagers
{
    public interface IOrderProcessManagerRepository
    {
        Task<OrderProcessManager> FindAsync(Guid aggregateId);
        Task Save(OrderProcessManager processManager);
    }

    public class OrderProcessManagerRepository :  IOrderProcessManagerRepository
    {
        private readonly IDispatcher _dispatcher;
        private readonly IUnitOfWork _unitOfWork;
        protected IDbTransaction _transaction => _unitOfWork.Transaction;
        protected IDbConnection _connection => _unitOfWork.Connection;

        public OrderProcessManagerRepository(IDispatcher dispatcher, IUnitOfWork unitOfWork)
        {
            _dispatcher = dispatcher;
            _unitOfWork = unitOfWork;
        }

        public async Task<OrderProcessManager> FindAsync(Guid aggregateId)
        {
            return await _connection.QueryFirstOrDefault(
                @"SELECT * FROM Transaction.OrderProcessManager where AggregateId = @AggregateId",
                new {AggregateId = aggregateId}, _transaction);
        }

        public async Task Save(OrderProcessManager processManager)
        {
            processManager.UpdateModifiedDate();
            var pm = await FindAsync(processManager.AggregateId);
            if (pm == null)
            {
                await Add(processManager);
            }
            else
            {
                await Update(processManager);
            }

            await HandleCommands(processManager);
        }

        private async Task Add(OrderProcessManager processManager)
        {
            await _connection.ExecuteAsync(
                @"INSERT INTO Transactions.OrderProcessManager(Id, AggregateId, WalletId, CompanyId, StockName, StockCode, StockUnit, StockQuantity, ChargedWalletAmount, CancelReason, State, CreatedAt, ModifiedAt)
                                            VALUES(@Id, @AggregateId, @WalletId, @CompanyId, @StockName, @StockCode, @StockUnit, @StockQuantity, @ChargedWalletAmount, @CancelReason, @State, @CreatedAt, @ModifiedAt)",
                new
                {
                    Id = Guid.NewGuid(),
                    AggregateId = processManager.AggregateId,
                    WalletId = processManager.WalletId,
                    CompanyId = processManager.CompanyId,
                    StockName = processManager.StockName,
                    StockCode = processManager.StockCode,
                    StockUnit = processManager.StockUnit,
                    StockQuantity = processManager.StockQuantity,
                    ChargedWalletAmount = processManager.ChargedWalletAmount,
                    CancelReason = processManager.CancelReason,
                    State = processManager.State,
                    CreatedAt = processManager.CreatedAt,
                    ModifiedAt = processManager.ModifiedAt
                }, _transaction);
        }

        private async Task Update(OrderProcessManager processManager)
        {
            await _connection.ExecuteAsync(@"UPDATE Transactions.OrderProcessManager
                                            SET WalletId = @WalletId,
                                            CompanyId = @CompanyId,
                                            StockName = @StockName,
                                            StockCode = @StockCode,
                                            StockUnit = @StockUnit,
                                            StockQuantity = @StockQuantity,
                                            ChargedWalletAmount = @ChargedWalletAmount,
                                            CancelReason = @CancelReason,
                                            State = @State,
                                            ModifiedAt = @ModifiedAt
                                            WHERE AggregateId = @AggregateId", new
            {
                WalletId = processManager.WalletId,
                CompanyId = processManager.CompanyId,
                StockName = processManager.StockName,
                StockCode = processManager.StockCode,
                StockUnit = processManager.StockUnit,
                StockQuantity = processManager.StockQuantity,
                ChargedWalletAmount = processManager.ChargedWalletAmount,
                CancelReason = processManager.CancelReason,
                State = processManager.State,
                ModifiedAt = processManager.ModifiedAt,
                AggregateId = processManager.AggregateId
            }, _transaction);
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
