using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AutoFixture;
using Stocqres.Core.Domain;
using Stocqres.Core.Events;
using Stocqres.Customers.Api.Investors.Events;
using Stocqres.Customers.Investors.Domain;
using Stocqres.Infrastructure.EventRepository;

namespace Stocqres.UnitTests.EventSourcing
{
    public class EventSourcingUnitTestsBase
    {
        protected readonly IFixture _fixture;
        protected readonly Guid _aggregateId;
        
        protected readonly Guid _userId;
        protected readonly Guid _walletId;
        protected readonly string _firstName;
        protected readonly string _lastName;
        protected readonly string _aggregateType;
        protected readonly Investor _investor;

        protected EventSourcingUnitTestsBase()
        {
            _fixture = new Fixture();

            _aggregateId = Guid.NewGuid();
            _userId = Guid.NewGuid();
            _walletId = Guid.NewGuid();
            _firstName = _fixture.Create<string>();
            _lastName = _fixture.Create<string>();
            _aggregateType = typeof(Investor).Name;
            _investor = RecreateInvestor();
        }

        protected IEnumerable<EventData> GetEventDatas()
        {
            var investorCreatedEvent = new InvestorCreatedEvent(_aggregateId, _userId, _firstName, _lastName);

            var walletToInvestorAssignedEvent = new WalletToInvestorAssignedEvent(_aggregateId, _walletId);

            var eventDatas = new List<EventData>
            {
                investorCreatedEvent.ToEventData(_aggregateId, _aggregateType, 1, DateTime.Now),
                walletToInvestorAssignedEvent.ToEventData(_aggregateId, _aggregateType, 2, DateTime.Now)
            };

            return eventDatas;
        }

        protected IEnumerable<IEvent> GetEvents()
        {
            var investorCreatedEvent = new InvestorCreatedEvent(_aggregateId, _userId, _firstName, _lastName);

            var walletToInvestorAssignedEvent = new WalletToInvestorAssignedEvent(_aggregateId, _walletId);

            var events = new List<IEvent>
            {
                investorCreatedEvent, walletToInvestorAssignedEvent
            };

            return events;
        }

        protected Investor RecreateInvestor()
        {
            var aggregateFactory = new AggregateRootFactory();

            var events = GetEventDatas().OrderBy(e => e.Version).Select(x => x.DeserializeEvent());

            return (Investor)aggregateFactory.CreateAsync<Investor>(events);
        }
    }
}
