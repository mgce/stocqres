using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Stocqres.Core.Dispatcher;
using Stocqres.Core.Events;
using Stocqres.Customers.Investors.Domain.Commands;
using Stocqres.SharedKernel.Events;

namespace Stocqres.Customers.Investors.Application
{
    public class InvestorsEventHandler : IEventHandler<UserForInvestorCreated>
    {
        private readonly IDispatcher _dispatcher;

        public InvestorsEventHandler(IDispatcher dispatcher)
        {
            _dispatcher = dispatcher;
        }
        public async Task HandleAsync(UserForInvestorCreated @event)
        {
            var command = new CreateInvestorCommand(@event.AggregateId, @event.FirstName, @event.LastName);
            await _dispatcher.SendAsync(command);
        }
    }
}
