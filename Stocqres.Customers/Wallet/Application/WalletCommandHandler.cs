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
using Stocqres.Infrastructure.ProjectionReader;
using Stocqres.SharedKernel.Commands;
using Stocqres.SharedKernel.Stocks;

namespace Stocqres.Customers.Wallet.Application
{
    public class WalletCommandHandler : ICommandHandler<CreateWalletCommand>, ICommandHandler<ChargeWalletCommand>, ICommandHandler<AddStockToWalletCommand>
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

        public async Task HandleAsync(ChargeWalletCommand command)
        {
            var company = await _eventRepository.GetByIdAsync<Company>(command.CompanyId);
            if(company == null)
                throw new StocqresException("Company doesn't exist");

            if(company.Stock == null)
                throw new StocqresException("Company doesn't have any stock");

            var wallet = await _eventRepository.GetByIdAsync<Domain.Wallet>(command.WalletId);
            if(wallet == null)
                throw new StocqresException("Wallet doesn't exist");

            var stockPrice = await _stockExchangeService.GetStockPrice(company.Stock.Code);
            wallet.ChargeWallet(stockPrice * command.Quantity);
            await _eventRepository.SaveAsync(wallet);
        }

        public async Task HandleAsync(AddStockToWalletCommand command)
        {
            var wallet = await _eventRepository.GetByIdAsync<Domain.Wallet>(command.WalletId);
            if (wallet == null)
                throw new StocqresException("Wallet doesn't exist");

            wallet.AddStock(command.StockName, command.StockCode, command.StockUnit, command.StockQuantity);

            await _eventRepository.SaveAsync(wallet);
        }
    }
}
