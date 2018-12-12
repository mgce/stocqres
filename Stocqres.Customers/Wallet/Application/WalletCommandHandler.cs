using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Stocqres.Core.Commands;
using Stocqres.Core.Exceptions;
using Stocqres.Customers.Companies.Domain;
using Stocqres.Customers.Investors.Domain;
using Stocqres.Customers.Investors.Presentation.Projections;
using Stocqres.Customers.Wallet.Commands;
using Stocqres.Infrastructure.EventRepository;
using Stocqres.Infrastructure.ExternalServices.StockExchangeService;
using Stocqres.Infrastructure.Projections;
using Stocqres.SharedKernel.Commands;
using Stocqres.SharedKernel.Stocks;

namespace Stocqres.Customers.Wallet.Application
{
    public class WalletCommandHandler : 
        ICommandHandler<CreateWalletCommand>, 
        ICommandHandler<ChargeWalletAmountCommand>, 
        ICommandHandler<AddStockToWalletCommand>,
        ICommandHandler<RollbackWalletChargeCommand>,
        ICommandHandler<TakeOffStocksFromWalletCommand>,
        ICommandHandler<TopUpWalletAmountCommand>,
    {
        private readonly IProjectionReader _projectionReader;
        private readonly IEventRepository _eventRepository;
        private readonly IStockExchangeService _stockExchangeService;

        public WalletCommandHandler(IProjectionReader projectionReader, IEventRepository eventRepository, IStockExchangeService stockExchangeService)
        {
            _projectionReader = projectionReader;
            _eventRepository = eventRepository;
            _stockExchangeService = stockExchangeService;
        }
        public async Task HandleAsync(CreateWalletCommand command)
        {
            var investor = await _eventRepository.GetByIdAsync<Investor>(command.InvestorId);
            if(investor == null)
                throw new StocqresException("Investor not exist");

            var wallet = new Domain.Wallet(command.InvestorId,  Currency.PLN, command.Amount);
            await _eventRepository.SaveAsync(wallet);
        }

        public async Task HandleAsync(ChargeWalletAmountCommand amountCommand)
        {
            var company = await _eventRepository.GetByIdAsync<Company>(amountCommand.CompanyId);
            if(company == null)
                throw new StocqresException("Company doesn't exist");

            if(company.Stock == null)
                throw new StocqresException("Company doesn't have any stock");

            var wallet = await GetWallet(amountCommand.WalletId);

            var stockPrice = await _stockExchangeService.GetStockPrice(company.Stock.Code);
            wallet.ChargeWallet(amountCommand.OrderId, stockPrice * amountCommand.Quantity);
            await _eventRepository.SaveAsync(wallet);
        }

        public async Task HandleAsync(AddStockToWalletCommand command)
        {
            var wallet = await GetWallet(command.WalletId);

            wallet.AddStock(command.OrderId, command.CompanyId, command.StockName, command.StockCode, command.StockUnit, command.StockQuantity);

            await _eventRepository.SaveAsync(wallet);
        }

        public async Task HandleAsync(RollbackWalletChargeCommand command)
        {
            var wallet = await GetWallet(command.WalletId);

            wallet.RollbackCharge(command.OrderId, command.AmountToRollback);

            await _eventRepository.SaveAsync(wallet);
        }

        private async Task<Domain.Wallet> GetWallet(Guid walletId)
        {
            var wallet = await _eventRepository.GetByIdAsync<Domain.Wallet>(walletId);
            if (wallet == null)
                throw new StocqresException("Wallet doesn't exist");
            return wallet;
        }

        public async Task HandleAsync(TakeOffStocksFromWalletCommand command)
        {
            var wallet = await GetWallet(command.WalletId);

            wallet.TakeOffStocks(command.CompanyId, command.Quantity);

            await _eventRepository.SaveAsync(wallet);
        }

        public async Task HandleAsync(TopUpWalletAmountCommand command)
        {
            var wallet = await GetWallet(command.WalletId);

            var stockPrice = await _stockExchangeService.GetStockPrice(command.StockCode);
            wallet.ChargeWallet(command.OrderId, stockPrice * command.Quantity);
            await _eventRepository.SaveAsync(wallet);
        }
    }
}
