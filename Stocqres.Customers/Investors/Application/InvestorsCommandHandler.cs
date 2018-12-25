using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Stocqres.Core.Commands;
using Stocqres.Core.Events;
using Stocqres.Customers.Api.Investors.Commands;
using Stocqres.Customers.Investors.Domain;
using Stocqres.Infrastructure.EventRepository;
using Stocqres.Infrastructure.Projections;

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
        }

 
    }
}
