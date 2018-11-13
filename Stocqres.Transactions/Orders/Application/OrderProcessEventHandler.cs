using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Stocqres.Core.Events;
using Stocqres.Core.Exceptions;
using Stocqres.SharedKernel.Events;
using Stocqres.Transactions.Infrastructure.ProcessManager;
using Stocqres.Transactions.Orders.Domain;
using Stocqres.Transactions.Orders.Domain.Events;
using Stocqres.Transactions.Orders.Domain.OrderProcessManager;

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
        private readonly IProcessManagerRepository _processManagerRepository;
        private OrderProcessManager _orderProcessManager {get; set; }

        public OrderProcessEventHandler(IProcessManagerRepository processManagerRepository)
        {
            _processManagerRepository = processManagerRepository;
        }

        public async Task HandleAsync(OrderCreatedEvent message)
        {
            var pm = await _processManagerRepository.Get<OrderProcessManager>(message.AggregateId);
            if(pm != null)
                throw new StocqresException("Process Manager for this Order currently exist");

            _orderProcessManager = new OrderProcessManager(message.AggregateId);

            _orderProcessManager.When(message);

            await _processManagerRepository.Save(_orderProcessManager);
        }

        public async Task HandleAsync(WalletChargedEvent message)
        {
            if (_orderProcessManager == null)
            {
                _orderProcessManager = await _processManagerRepository.Get<OrderProcessManager>(message.AggregateId);
            }

            try
            {
                _orderProcessManager.When(message);
            }
            catch (StocqresException e)
            {

                _orderProcessManager.When(new OrderCancelledEvent(e.Message));
            }

            await _processManagerRepository.Save(_orderProcessManager);
        }

        public async Task HandleAsync(CompanyChargedEvent message)
        {
            if (_orderProcessManager == null)
            {
                _orderProcessManager = await _processManagerRepository.Get<OrderProcessManager>(message.AggregateId);
            }

            try
            {
                _orderProcessManager.When(message);
            }
            catch (StocqresException e)
            {
                _orderProcessManager.When(new CompanyChargeFailedEvent(e.Message));
            }

            await _processManagerRepository.Save(_orderProcessManager);
        }

        public async Task HandleAsync(CompanyChargeFailedEvent message)
        {
            if (_orderProcessManager == null)
            {
                _orderProcessManager = await _processManagerRepository.Get<OrderProcessManager>(message.AggregateId);
            }
            _orderProcessManager.When(message);
            await _processManagerRepository.Save(_orderProcessManager);
        }

        public async Task HandleAsync(WalletChargeRollbackedEvent message)
        {
            if (_orderProcessManager == null)
            {
                _orderProcessManager = await _processManagerRepository.Get<OrderProcessManager>(message.AggregateId);
            }
            _orderProcessManager.When(message);
            await _processManagerRepository.Save(_orderProcessManager);
        }

        public async Task HandleAsync(OrderCancelledEvent message)
        {
            if (_orderProcessManager == null)
            {
                _orderProcessManager = await _processManagerRepository.Get<OrderProcessManager>(message.AggregateId);
            }
            _orderProcessManager.When(message);
            await _processManagerRepository.Save(_orderProcessManager);
        }

        public async Task HandleAsync(StockToWalletAddedEvent message)
        {
            await Act<StockToWalletAddedEvent>(message.AggregateId, () => _orderProcessManager.When(message));
        }

        private async Task Act<T>(Guid aggregateId, Action action)
        {
            if (_orderProcessManager == null)
            {
                _orderProcessManager = await _processManagerRepository.Get<OrderProcessManager>(aggregateId);
            }

            action();

            await _processManagerRepository.Save(_orderProcessManager);
        }

        
    }
}
