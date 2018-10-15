using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Stocqres.Core.Commands;
using Stocqres.Core.Exceptions;
using Stocqres.Customers.Investors.Domain;
using Stocqres.Customers.Investors.Presentation.Projections;
using Stocqres.Customers.Wallet.Commands;
using Stocqres.Infrastructure.EventRepository;
using Stocqres.Infrastructure.ProjectionReader;

namespace Stocqres.Customers.Wallet.Application
{
    public class WalletCommandHandler : ICommandHandler<CreateWalletCommand>
    {
        private readonly IProjectionReader _projectionReader;
        private readonly IEventRepository _eventRepository;

        public WalletCommandHandler(IProjectionReader projectionReader, IEventRepository eventRepository)
        {
            _projectionReader = projectionReader;
            _eventRepository = eventRepository;
        }
        public async Task HandleAsync(CreateWalletCommand command)
        {
            var investor = await _eventRepository.GetByIdAsync<Investor>(command.InvestorId);
            if(investor == null)
                throw new StocqresException("Investor not exist");

            var wallet = new Domain.Wallet(command.InvestorId,  Currency.PLN, command.Amount);
            await _eventRepository.SaveAsync(wallet);
        }
    }
}
