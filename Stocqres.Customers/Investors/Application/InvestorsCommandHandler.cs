using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Stocqres.Core.Commands;
using Stocqres.Core.Events;
using Stocqres.Customers.Investors.Domain;
using Stocqres.Customers.Investors.Domain.Commands;
using Stocqres.Customers.Investors.Domain.Events;
using Stocqres.Customers.Investors.Presentation.Projections;
using Stocqres.Infrastructure.EventRepository;
using Stocqres.Infrastructure.ProjectionReader;

namespace Stocqres.Customers.Investors.Application
{
    public class InvestorsCommandHandler : ICommandHandler<CreateInvestorCommand>
    {
        private readonly IEventRepository _eventRepository;
        private readonly IEventBus _eventBus;
        private readonly IProjectionReader _projectionReader;

        public InvestorsCommandHandler(IEventRepository eventRepository, IEventBus eventBus, IProjectionReader projectionReader)
        {
            _eventRepository = eventRepository;
            _eventBus = eventBus;
            _projectionReader = projectionReader;
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
