using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Stocqres.Core.Commands;
using Stocqres.Core.Events;
using Stocqres.Domain;
using Stocqres.Domain.Commands;
using Stocqres.Domain.Enums;
using Stocqres.Domain.Events.StockGroup;
using Stocqres.Domain.Exceptions.Stock;
using Stocqres.Domain.Exceptions.StockExchange;
using Stocqres.Domain.Exceptions.User;
using Stocqres.Infrastructure;
using Stocqres.Infrastructure.Repositories.Api;

namespace Stocqres.Application.Stock.Handlers
{
    public class StockCommandHandler : ICommandHandler<BuyStockCommand>
    {
        private readonly IStockRepository _stockRepository;
        private readonly IUserRepository _userRepository;
        private readonly IStockExchangeRepository _stockExchangeRepository;
        private readonly IStockGroupRepository _stockGroupRepository;
        private readonly IEventBus _eventBus;

        public StockCommandHandler(IStockRepository stockRepository, 
            IUserRepository userRepository, 
            IStockExchangeRepository stockExchangeRepository, 
            IStockGroupRepository stockGroupRepository, 
            IEventBus eventBus)
        {
            _stockRepository = stockRepository;
            _userRepository = userRepository;
            _stockExchangeRepository = stockExchangeRepository;
            _stockGroupRepository = stockGroupRepository;
            _eventBus = eventBus;
        }
        public async Task HandleAsync(BuyStockCommand command)
        {
            var user = await _userRepository.GetUserAsync(command.UserId);
            if(user == null)
                throw new UserDoesNotExistException();

            var stock = await _stockRepository.GetAsync(command.StockId);
            if(stock == null)
                throw new StockExchangeDoesNotExistException();

            var stockExchange = await _stockExchangeRepository.GetAsync(stock.StockExchangeId);
            if(stockExchange == null)
                throw new StockExchangeDoesNotExistException();

            var stockExchangeStockGroup = await _stockGroupRepository.GetAsync(x =>
                x.OwnerId == stockExchange.Id && 
                x.StockOwner == StockOwner.StockExchange &&
                x.StockId == command.StockId);

            if (stockExchangeStockGroup.Quantity < command.Quantity)
                throw new StockExchangeDoesNotHaveEnoughtStocksException();

            stockExchangeStockGroup.DecreaseQuantity(command.Quantity);
            await _eventBus.Publish(new StockGroupQuantityChangedEvent(stockExchangeStockGroup.Id,
                stockExchangeStockGroup.Quantity));

            var userStockGroup = new StockGroup(user.Id, StockOwner.User, command.Quantity, stock.Id);
            await _eventBus.Publish(new StockGroupCreatedEvent(userStockGroup.Id, userStockGroup.OwnerId, userStockGroup.StockOwner,
                userStockGroup.Quantity, userStockGroup.StockId));

            await _stockGroupRepository.UpdateAsync(userStockGroup, stockExchangeStockGroup);

        }
    }
}
