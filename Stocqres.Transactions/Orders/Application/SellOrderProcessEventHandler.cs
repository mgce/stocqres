using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Stocqres.Core.Events;
using Stocqres.Core.Exceptions;
using Stocqres.Customers.Api.Companies.Commands;
using Stocqres.Customers.Api.Wallet.Events;
using Stocqres.SharedKernel.Events;
using Stocqres.Transactions.Infrastructure.ProcessManagers;
using Stocqres.Transactions.Orders.Domain;
using Stocqres.Transactions.Orders.Domain.Events;
using Stocqres.Transactions.Orders.Domain.ProcessManagers;

namespace Stocqres.Transactions.Orders.Application
{
    public class SellOrderProcessEventHandler : 
        IEventHandler<SellOrderCreatedEvent>, 
        IEventHandler<StocksTakedOffFromWalletEvent>, 
        IEventHandler<StocksAddedToCompanyEvent>, 
        IEventHandler<WalletAmountToppedUpEvent>,
        IEventHandler<SellOrderFinishedEvent>
    {
        private readonly ISellOrderProcessManagerRepository _processManagerRepository;

        public SellOrderProcessEventHandler(ISellOrderProcessManagerRepository processManagerRepository)
        {
            _processManagerRepository = processManagerRepository;
        }

        public async Task HandleAsync(SellOrderCreatedEvent message)
        {
            var pm = await _processManagerRepository.FindAsync(message.AggregateId);
            if(pm != null)
                throw new StocqresException("Process Manager for this Order currently exist");

            var orderProcessManager = new SellOrderProcessManager(message.AggregateId);

            orderProcessManager.When(message);

            await _processManagerRepository.Save(orderProcessManager);
        }

        public async Task HandleAsync(StocksTakedOffFromWalletEvent message)
        {
            var orderProcessManager = await _processManagerRepository.FindAsync(message.OrderId);

            try
            {
                orderProcessManager.When(message);
            }
            catch (StocqresException e)
            {

                orderProcessManager.When(new SellOrderCancelledEvent(e.Message));
            }

            await _processManagerRepository.Save(orderProcessManager);
        }

        public async Task HandleAsync(StocksAddedToCompanyEvent message)
        {
            var orderProcessManager = await _processManagerRepository.FindAsync(message.OrderId);

            try
            {
                orderProcessManager.When(message);
            }
            catch (StocqresException e)
            {
                orderProcessManager.When(new SellOrderCancelledEvent(e.Message));
            }

            await _processManagerRepository.Save(orderProcessManager);
        }

        public async Task HandleAsync(WalletAmountToppedUpEvent message)
        {
            var orderProcessManager = await _processManagerRepository.FindAsync(message.OrderId);

            orderProcessManager.When(message);

            await _processManagerRepository.Save(orderProcessManager);
        }

        public async Task HandleAsync(SellOrderFinishedEvent message)
        {
            var orderProcessManager = await _processManagerRepository.FindAsync(message.AggregateId);

            orderProcessManager.When(message);

            await _processManagerRepository.Save(orderProcessManager);
        }

    }
}
