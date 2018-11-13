using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Stocqres.Core.Commands;
using Stocqres.Core.Exceptions;
using Stocqres.Infrastructure.EventRepository;
using Stocqres.Transactions.Orders.Domain;
using Stocqres.Transactions.Orders.Domain.Command;

namespace Stocqres.Transactions.Orders.Application
{
    public class OrderCommandHandler : 
        ICommandHandler<CreateOrderCommand>,
        ICommandHandler<CancelOrderCommand>,
        ICommandHandler<FinishOrderCommand>
    {
        private readonly IEventRepository _eventRepository;

        public OrderCommandHandler(IEventRepository eventRepository)
        {
            _eventRepository = eventRepository;
        }
        public async Task HandleAsync(CreateOrderCommand command)
        {
            var order = new Order(command.WalletId, command.CompanyId, command.Quantity);
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

        public async Task HandleAsync(FinishOrderCommand command)
        {
            var order = await _eventRepository.GetByIdAsync<Order>(command.OrderId);
            if (order == null)
                throw new StocqresException("Order does not exist");

            order.FinishOrder();

            await _eventRepository.SaveAsync(order);
        }

    }
}
