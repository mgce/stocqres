using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Stocqres.Core.Commands;
using Stocqres.Core.Events;
using Stocqres.Customers.Investors.Domain;
using Stocqres.Customers.Investors.Domain.Commands;
using Stocqres.Customers.Investors.Domain.Events;
using Stocqres.Infrastructure.EventRepository;

namespace Stocqres.Customers.Investors.Application
{
    public class InvestorsCommandHandler : ICommandHandler<CreateInvestorCommand>
    {
        private readonly IEventRepository _eventRepository;
        private readonly IEventBus _eventBus;

        public InvestorsCommandHandler(IEventRepository eventRepository, IEventBus eventBus)
        {
            _eventRepository = eventRepository;
            _eventBus = eventBus;
        }
        public async Task HandleAsync(CreateInvestorCommand command)
        {
            var investor = new Investor(command.UserId, command.FirstName, command.LastName);
            await _eventRepository.SaveAsync(investor);
            await _eventBus.Publish(new InvestorCreatedEvent(investor.Id, investor.UserId, investor.FirstName,
                investor.LastName));
        }
    }
}
