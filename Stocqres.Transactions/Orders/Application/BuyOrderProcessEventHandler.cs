﻿using System;
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
    public class BuyOrderProcessEventHandler : 
        IEventHandler<BuyOrderCreatedEvent>, 
        IEventHandler<WalletChargedEvent>, 
        IEventHandler<CompanyChargedEvent>, 
        IEventHandler<StockToWalletAddedEvent>,
        IEventHandler<CompanyChargeFailedEvent>,
        IEventHandler<WalletChargeRollbackedEvent>,
        IEventHandler<BuyOrderCancelledEvent>
    {
        private readonly IBuyOrderProcessManagerRepository _processManagerRepository;

        public BuyOrderProcessEventHandler(IBuyOrderProcessManagerRepository processManagerRepository)
        {
            _processManagerRepository = processManagerRepository;
        }

        public async Task HandleAsync(BuyOrderCreatedEvent message)
        {
            var pm = await _processManagerRepository.FindAsync(message.AggregateId);
            if(pm != null)
                throw new StocqresException("Process Manager for this Order currently exist");

            var orderProcessManager = new BuyOrderProcessManager(message.AggregateId);

            orderProcessManager.When(message);

            await _processManagerRepository.Save(orderProcessManager);
        }

        public async Task HandleAsync(WalletChargedEvent message)
        {
            var orderProcessManager = await _processManagerRepository.FindAsync(message.OrderId);

            try
            {
                orderProcessManager.When(message);
            }
            catch (StocqresException e)
            {

                orderProcessManager.When(new BuyOrderCancelledEvent(e.Message));
            }

            await _processManagerRepository.Save(orderProcessManager);
        }

        public async Task HandleAsync(CompanyChargedEvent message)
        {
            var orderProcessManager = await _processManagerRepository.FindAsync(message.OrderId);

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
            var orderProcessManager = await _processManagerRepository.FindAsync(message.OrderId);

            orderProcessManager.When(message);

            await _processManagerRepository.Save(orderProcessManager);
        }

        public async Task HandleAsync(WalletChargeRollbackedEvent message)
        {
            var orderProcessManager = await _processManagerRepository.FindAsync(message.OrderId);

            orderProcessManager.When(message);

            await _processManagerRepository.Save(orderProcessManager);
        }

        public async Task HandleAsync(BuyOrderCancelledEvent message)
        {
            var orderProcessManager = await _processManagerRepository.FindAsync(message.AggregateId);

            orderProcessManager.When(message);

            await _processManagerRepository.Save(orderProcessManager);
        }

        public async Task HandleAsync(StockToWalletAddedEvent message)
        {
            var orderProcessManager = await _processManagerRepository.FindAsync(message.OrderId);

            orderProcessManager.When(message);

            await _processManagerRepository.Save(orderProcessManager);
        }

        private async Task Act<T>(Guid aggregateId, Action action)
        {
            var orderProcessManager = await _processManagerRepository.FindAsync(aggregateId);

            action();

            await _processManagerRepository.Save(orderProcessManager);
        }
    }
}
