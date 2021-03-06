﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Stocqres.Core.Commands;
using Stocqres.Core.Exceptions;
using Stocqres.Infrastructure.EventRepository;
using Stocqres.Transactions.Orders.Domain;
using Stocqres.Transactions.Orders.Domain.Command;
using Stocqres.Transactions.Orders.Domain.Enums;
using Stocqres.Transactions.Orders.Domain.Order;
using Stocqres.Transactions.Orders.Domain.Order.Factories;

namespace Stocqres.Transactions.Orders.Application
{
    public class OrderCommandHandler : 
        ICommandHandler<CreateBuyOrderCommand>,
        ICommandHandler<CancelOrderCommand>,
        ICommandHandler<FinishBuyOrderCommand>,
        ICommandHandler<CreateSellOrderCommand>,
        ICommandHandler<FinishSellOrderCommand>
    {
        private readonly IEventRepository _eventRepository;
        private readonly IOrderFactory _orderFactory;

        public OrderCommandHandler(IEventRepository eventRepository, IOrderFactory orderFactory)
        {
            _eventRepository = eventRepository;
            _orderFactory = orderFactory;
        }
        public async Task HandleAsync(CreateBuyOrderCommand command)
        {
            var order = _orderFactory.CreateBuyOrder(command.WalletId, command.CompanyId, command.Quantity);
            await _eventRepository.SaveAsync(order);
        }

        public async Task HandleAsync(CancelOrderCommand command)
        {
            var order = await _eventRepository.GetByIdAsync<Order>(command.OrderId);
            if(order == null)
                throw new StocqresException("Order does not exist");

            order.CancelOrder(command.CancelReason);

            await _eventRepository.SaveAsync(order);
        }

        public async Task HandleAsync(FinishBuyOrderCommand command)
        {
            var order = await _eventRepository.GetByIdAsync<BuyOrder>(command.OrderId);
            if (order == null)
                throw new StocqresException("Order does not exist");

            order.FinishOrder();

            await _eventRepository.SaveAsync(order);
        }

        public async Task HandleAsync(CreateSellOrderCommand command)
        {
            var order = _orderFactory.CreateSellOrder(command.WalletId, command.CompanyId, command.Quantity, command.StockCode);
            await _eventRepository.SaveAsync(order);
        }

        public async Task HandleAsync(FinishSellOrderCommand command)
        {
            var order = await _eventRepository.GetByIdAsync<SellOrder>(command.OrderId);
            if (order == null)
                throw new StocqresException("Order does not exist");

            order.FinishOrder();

            await _eventRepository.SaveAsync(order);
        }
    }
}
