using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Stocqres.Application.StockExchange.Services;
using Stocqres.Core.Commands;
using Stocqres.Core.Events;
using Stocqres.Domain;
using Stocqres.Domain.Commands;
using Stocqres.Domain.Enums;
using Stocqres.Domain.Events.StockGroup;
using Stocqres.Domain.Events.Wallet;
using Stocqres.Domain.Exceptions.Stock;
using Stocqres.Domain.Exceptions.StockExchange;
using Stocqres.Domain.Exceptions.User;
using Stocqres.Infrastructure;
using Stocqres.Infrastructure.Repositories.Api;

namespace Stocqres.Application.Stock.Handlers
{
    public class StockCommandHandler : ICommandHandler<BuyStocksCommand>, ICommandHandler<SellStocksCommand>
    {
        private readonly IStockRepository _stockRepository;
        private readonly IUserRepository _userRepository;
        private readonly IStockExchangeRepository _stockExchangeRepository;
        private readonly IStockGroupRepository _stockGroupRepository;
        private readonly IEventBus _eventBus;
        private readonly IStockExchangeService _stockExchangeService;
        private readonly IWalletRepository _walletRepository;

        public StockCommandHandler(IStockRepository stockRepository, 
            IUserRepository userRepository, 
            IStockExchangeRepository stockExchangeRepository, 
            IStockGroupRepository stockGroupRepository, 
            IEventBus eventBus, 
            IStockExchangeService stockExchangeService, 
            IWalletRepository walletRepository)
        {
            _stockRepository = stockRepository;
            _userRepository = userRepository;
            _stockExchangeRepository = stockExchangeRepository;
            _stockGroupRepository = stockGroupRepository;
            _eventBus = eventBus;
            _stockExchangeService = stockExchangeService;
            _walletRepository = walletRepository;
        }
        public async Task HandleAsync(BuyStocksCommand command)
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

            var stockExchangeStockGroup =
                await _stockGroupRepository.GetStockGroupFromStockExchange(stockExchange.Id, stock.Id);

            if (stockExchangeStockGroup.Quantity < command.Quantity)
                throw new StockExchangeDoesNotHaveEnoughtStocksException();

            stockExchangeStockGroup.DecreaseQuantity(command.Quantity);
            await _eventBus.Publish(new StockGroupQuantityChangedEvent(stockExchangeStockGroup.Id,
                stockExchangeStockGroup.Quantity));

            var stockPrice = await _stockExchangeService.GetStockPrice(stock.Code);

            await BurdenUserWallet(user.WalletId, stockPrice, stock.Unit, command.Quantity);

            var userStockGroup = await _stockGroupRepository.GetUserStockGroup(command.UserId, command.StockId);
            if (userStockGroup == null)
            {
                userStockGroup = await CreateStockGroup(user.Id, stock.Id, stockPrice, command.Quantity);
            }
            else
            {
                userStockGroup.IncreaseQuantity(command.Quantity);
                await _eventBus.Publish(new StockGroupQuantityChangedEvent(userStockGroup.Id,
                    userStockGroup.Quantity));
            }
            await _stockGroupRepository.UpdateAsync(stockExchangeStockGroup, userStockGroup);
        }

        public async Task BurdenUserWallet(Guid walletId, decimal stockPrice, int unit, int quantity)
        {
            var userWallet = await _walletRepository.GetAsync(walletId);
            if (userWallet.HaveEnoughtMoney(stockPrice, unit, quantity))
                throw new Exception("User doesn't have enough money to buy this stocks");

            var moneyToSpend = stockPrice * unit * quantity;
            userWallet.DecreaseAmount(moneyToSpend);
            await _walletRepository.UpdateAsync(userWallet);
            await _eventBus.Publish(new WalletAmountDecreasedEvent(userWallet.Id, userWallet.Amount));
        }

        public async Task UnloadUserWallet(Guid walletId, decimal stockPrice, int unit, int quantity)
        {
            var userWallet = await _walletRepository.GetAsync(walletId);
            if (userWallet.HaveEnoughtMoney(stockPrice, unit, quantity))
                throw new Exception("User doesn't have enough money to buy this stocks");

            var moneyToSpend = stockPrice * unit * quantity;
            userWallet.IncreaseAmount(moneyToSpend);
            await _walletRepository.UpdateAsync(userWallet);
            await _eventBus.Publish(new WalletAmountIncreasedEvent(userWallet.Id, userWallet.Amount));
        }

        public async Task<Domain.StockGroup> CreateStockGroup(Guid userId, Guid stockId, decimal stockPrice, int quantity)
        {
            var userStockGroup = new Domain.StockGroup(userId, StockOwner.User, quantity, stockPrice, stockId);
            await _eventBus.Publish(new StockGroupCreatedEvent(userStockGroup.Id, userStockGroup.OwnerId, userStockGroup.StockOwner,
                userStockGroup.Quantity, userStockGroup.StockId));
            return userStockGroup;
        }

        public async Task HandleAsync(SellStocksCommand command)
        {
            var user = await _userRepository.GetUserAsync(command.UserId);
            if (user == null)
                throw new UserDoesNotExistException();

            var stock = await _stockRepository.GetAsync(command.StockId);
            if (stock == null)
                throw new StockExchangeDoesNotExistException();

            var stockExchange = await _stockExchangeRepository.GetAsync(stock.StockExchangeId);
            if (stockExchange == null)
                throw new StockExchangeDoesNotExistException();

            var stockPrice = await _stockExchangeService.GetStockPrice(stock.Code);

            await BurdenUserWallet(user.WalletId, stockPrice, stock.Unit, command.Quantity);

            var userStockGroup = await _stockGroupRepository.GetUserStockGroup(command.UserId, command.StockId);
            var stockExchangeStockGroup =
                await _stockGroupRepository.GetStockGroupFromStockExchange(stockExchange.Id, stock.Id);

            userStockGroup.DecreaseQuantity(command.Quantity);
            await _eventBus.Publish(new StockGroupQuantityChangedEvent(userStockGroup.Id,
                userStockGroup.Quantity));

            stockExchangeStockGroup.DecreaseQuantity(command.Quantity);
            await _eventBus.Publish(new StockGroupQuantityChangedEvent(stockExchangeStockGroup.Id,
                stockExchangeStockGroup.Quantity));

            await _stockGroupRepository.UpdateAsync(stockExchangeStockGroup, userStockGroup);
        }

    }
}
