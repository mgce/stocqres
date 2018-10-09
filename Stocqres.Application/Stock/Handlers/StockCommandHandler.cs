using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Stocqres.Application.StockExchange.Services;
using Stocqres.Application.StockGroup.Services;
using Stocqres.Core.Commands;
using Stocqres.Core.Events;
using Stocqres.Core.EventStore;
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
        private readonly IStockGroupRepository _stockGroupRepository;
        private readonly IEventBus _eventBus;
        private readonly IStockExchangeService _stockExchangeService;
        private readonly IWalletRepository _walletRepository;
        private readonly ICustomEventStore _eventStore;
        private readonly IStockGroupService _stockGroupService;

        public StockCommandHandler(
            IStockGroupRepository stockGroupRepository, 
            IEventBus eventBus, 
            IStockExchangeService stockExchangeService, 
            IWalletRepository walletRepository, 
            ICustomEventStore eventStore,
            IStockGroupService stockGroupService)
        {
            _stockGroupRepository = stockGroupRepository;
            _eventBus = eventBus;
            _stockExchangeService = stockExchangeService;
            _walletRepository = walletRepository;
            _eventStore = eventStore;
            _stockGroupService = stockGroupService;
        }
        public async Task HandleAsync(BuyStocksCommand command)
        {
            var user = await _eventStore.Load<Domain.User>(command.UserId);
            var stock = await _eventStore.Load<Domain.Stock>(command.StockId);
            var stockExchange = await _eventStore.Load<Domain.StockExchange>(stock.StockExchangeId);

            var stockExchangeStockGroup = await _stockGroupService.GetStockGroupFromStockExchange(stockExchange.Id, stock.Id);

            if (stockExchangeStockGroup.Quantity < command.Quantity)
                throw new StockExchangeDoesNotHaveEnoughtStocksException();

            stockExchangeStockGroup.DecreaseQuantity(command.Quantity);
            await _eventBus.Publish(new StockGroupQuantityChangedEvent(stockExchangeStockGroup.Id,
                stockExchangeStockGroup.Quantity));

            var stockPrice = await _stockExchangeService.GetStockPrice(stock.Code);

            await BurdenUserWallet(user.WalletId, stockPrice, stock.Unit, command.Quantity);

            var userStockGroup = await _stockGroupService.GetUserStockGroup(command.UserId, command.StockId);
            if (userStockGroup == null)
            {
                 await CreateStockGroup(user.Id, stock.Id, stockPrice, command.Quantity);
                await _stockGroupRepository.UpdateAsync(stockExchangeStockGroup);
            }
            else
            {
                //Usunac _stockGroupRepository poniewaz z event handlera zmieniana jest wartosc w read modelu
                //przemyslec w jaki sposob roznica kwot powinna byc zapisana w evencie
                //tj. czy powinnismy zapisywać jedynie wartość ktora odejmujemy, czy wartosc koncowa
                userStockGroup.IncreaseQuantity(command.Quantity);
                await _eventBus.Publish(new StockGroupQuantityChangedEvent(userStockGroup.Id,
                    userStockGroup.Quantity));
                await _stockGroupRepository.UpdateAsync(stockExchangeStockGroup, userStockGroup);
            }
            
        }

        public async Task BurdenUserWallet(Guid walletId, decimal stockPrice, int unit, int quantity)
        {
            var userWallet = await _walletRepository.GetByIdAsync(walletId);
            if (userWallet.HaveEnoughtMoney(stockPrice, unit, quantity))
                throw new Exception("UserCodes doesn't have enough money to buy this stocks");

            var moneyToSpend = stockPrice * unit * quantity;
            userWallet.DecreaseAmount(moneyToSpend);
            await _walletRepository.UpdateAsync(userWallet);
            await _eventBus.Publish(new WalletAmountDecreasedEvent(userWallet.Id, userWallet.Amount));
        }

        public async Task UnloadUserWallet(Guid walletId, decimal stockPrice, int unit, int quantity)
        {
            var userWallet = await _walletRepository.GetByIdAsync(walletId);
            if (userWallet.HaveEnoughtMoney(stockPrice, unit, quantity))
                throw new Exception("UserCodes doesn't have enough money to buy this stocks");

            var moneyToSpend = stockPrice * unit * quantity;
            userWallet.IncreaseAmount(moneyToSpend);
            await _walletRepository.UpdateAsync(userWallet);
            await _eventBus.Publish(new WalletAmountIncreasedEvent(userWallet.Id, userWallet.Amount));
        }

        public async Task CreateStockGroup(Guid userId, Guid stockId, decimal stockPrice, int quantity)
        {
            var userStockGroup = new Domain.StockGroup(userId, StockOwner.User, quantity, stockPrice, stockId);
            await _eventBus.Publish(new StockGroupCreatedEvent(userStockGroup.Id, userStockGroup.OwnerId, userStockGroup.StockOwner,
                userStockGroup.Quantity, userStockGroup.StockId, stockPrice));
            await _stockGroupRepository.CreateAsync(userStockGroup);
        }

        public async Task HandleAsync(SellStocksCommand command)
        {
            var user = await _eventStore.Load<Domain.User>(command.UserId);
            var stock = await _eventStore.Load<Domain.Stock>(command.StockId);
            var stockExchange = await _eventStore.Load<Domain.StockExchange>(stock.StockExchangeId);

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
