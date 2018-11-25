using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Stocqres.Core.Events;
using Stocqres.Core.Exceptions;
using Stocqres.SharedKernel.Events;
using Stocqres.Transactions.Infrastructure.ProcessManagers;
using Stocqres.Transactions.Orders.Domain;
using Stocqres.Transactions.Orders.Domain.Events;
using Stocqres.Transactions.Orders.Domain.ProcessManagers;

namespace Stocqres.Transactions.Orders.Application
{
    public class OrderProcessEventHandler : 
        IEventHandler<OrderCreatedEvent>, 
        IEventHandler<WalletChargedEvent>, 
        IEventHandler<CompanyChargedEvent>, 
        IEventHandler<StockToWalletAddedEvent>,
        IEventHandler<CompanyChargeFailedEvent>,
        IEventHandler<WalletChargeRollbackedEvent>,
        IEventHandler<OrderCancelledEvent>
    {
        private readonly IOrderProcessManagerRepository _processManagerRepository;

        public OrderProcessEventHandler(IOrderProcessManagerRepository processManagerRepository)
        {
            _processManagerRepository = processManagerRepository;
        }

        public async Task HandleAsync(OrderCreatedEvent message)
        {
            var pm = await _processManagerRepository.Get(message.AggregateId);
            if(pm != null)
                throw new StocqresException("Process Manager for this Order currently exist");

            var orderProcessManager = new OrderProcessManager(message.AggregateId);

            orderProcessManager.When(message);

            await _processManagerRepository.Save(orderProcessManager);
        }

        public async Task HandleAsync(WalletChargedEvent message)
        {
            var orderProcessManager = await _processManagerRepository.Get(message.OrderId);

            try
            {
                orderProcessManager.When(message);
            }
            catch (StocqresException e)
            {

                orderProcessManager.When(new OrderCancelledEvent(e.Message));
            }

            await _processManagerRepository.Save(orderProcessManager);
        }

        public async Task HandleAsync(CompanyChargedEvent message)
        {
            var orderProcessManager = await _processManagerRepository.Get(message.OrderId);

            try
            {
                orderProcessManager.When(message);
            }
            catch (StocqresException e)
            {
                orderProcessManager.When(new CompanyChargeFailedEvent(message.OrderId, e.Message));
            }

            await _processManagerRepository.Save(orderProcessManager);
        }

        public async Task HandleAsync(CompanyChargeFailedEvent message)
        {
            var orderProcessManager = await _processManagerRepository.Get(message.OrderId);

            orderProcessManager.When(message);

            await _processManagerRepository.Save(orderProcessManager);
        }

        public async Task HandleAsync(WalletChargeRollbackedEvent message)
        {
            var orderProcessManager = await _processManagerRepository.Get(message.OrderId);

            orderProcessManager.When(message);

            await _processManagerRepository.Save(orderProcessManager);
        }

        public async Task HandleAsync(OrderCancelledEvent message)
        {
            var orderProcessManager = await _processManagerRepository.Get(message.AggregateId);

            orderProcessManager.When(message);

            await _processManagerRepository.Save(orderProcessManager);
        }

        public async Task HandleAsync(StockToWalletAddedEvent message)
        {
            var orderProcessManager = await _processManagerRepository.Get(message.OrderId);

            orderProcessManager.When(message);

            await _processManagerRepository.Save(orderProcessManager);
        }

        private async Task Act<T>(Guid aggregateId, Action action)
        {
            var orderProcessManager = await _processManagerRepository.Get(aggregateId);

            action();

            await _processManagerRepository.Save(orderProcessManager);
        }

        
    }
}
