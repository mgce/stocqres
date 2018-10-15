using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Stocqres.Core.Dispatcher;
using Stocqres.Core.Events;
using Stocqres.Core.Exceptions;
using Stocqres.Customers.Investors.Domain;
using Stocqres.Customers.Investors.Domain.Commands;
using Stocqres.Customers.Wallet.Events;
using Stocqres.Infrastructure.EventRepository;
using Stocqres.SharedKernel.Events;

namespace Stocqres.Customers.Investors.Application
{
    public class InvestorsEventHandler : IEventHandler<UserForInvestorCreated>, IEventHandler<WalletCreatedEvent>
    {
        private readonly IDispatcher _dispatcher;
        private readonly IEventRepository _eventRepository;

        public InvestorsEventHandler(IDispatcher dispatcher, IEventRepository eventRepository)
        {
            _dispatcher = dispatcher;
            _eventRepository = eventRepository;
        }
        public async Task HandleAsync(UserForInvestorCreated @event)
        {
            var command = new CreateInvestorCommand(@event.AggregateId, @event.FirstName, @event.LastName);
            await _dispatcher.SendAsync(command);
        }

        public async Task HandleAsync(WalletCreatedEvent @event)
        {
            var investor = await _eventRepository.GetByIdAsync<Investor>(@event.InvestorId);
            if(investor == null)
                throw new StocqresException("Investor doesn't exist");
            investor.AssignWallet(@event.AggregateId);
            await _eventRepository.SaveAsync(investor);
        }
    }
}
