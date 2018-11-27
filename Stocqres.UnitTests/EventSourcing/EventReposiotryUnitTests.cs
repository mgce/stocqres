using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoFixture;
using Dapper;
using FluentAssertions;
using NSubstitute;
using Stocqres.Core.Domain;
using Stocqres.Core.Events;
using Stocqres.Customers.Investors.Domain;
using Stocqres.Customers.Investors.Domain.Events;
using Stocqres.Infrastructure.DatabaseProvider;
using Stocqres.Infrastructure.EventRepository;
using Stocqres.Infrastructure.UnitOfWork;
using Xunit;

namespace Stocqres.UnitTests.EventSourcing
{
    public class EventReposiotryUnitTests : EventSourcingUnitTestsBase
    {
        private readonly IFixture _fixture;
        private readonly IAggregateRootFactory _aggregateRootFactory;
        private readonly IEventBus _eventBus;
        private readonly IDatabaseProvider _databaseProvider;
        private readonly IEventRepository _eventRepository;

        public EventReposiotryUnitTests()
        {
            _fixture = new Fixture();
            _aggregateRootFactory = Substitute.For<IAggregateRootFactory>();
            _eventBus = Substitute.For<IEventBus>();
            _databaseProvider = Substitute.For<IDatabaseProvider>();
            _eventRepository = new EventRepository(_aggregateRootFactory, _eventBus, _databaseProvider);
        }

        [Fact]
        public async void GetByIdAsync_WithExistingInvestor_ShouldReturnInvestorAggregate()
        {
            var eventToReturn = GetEvents();
            _databaseProvider.QueryAsync<EventData>(Arg.Any<string>()).ReturnsForAnyArgs(eventToReturn);

            var deserializedEvents = GetEvents().OrderBy(e => e.Version).Select(x => x.DeserializeEvent());

            _aggregateRootFactory
                .CreateAsync<Investor>(deserializedEvents)
                .ReturnsForAnyArgs(_investor);

            var aggregate = await _eventRepository.GetByIdAsync<Investor>(_aggregateId);

            aggregate.Should().Be(_investor);
            aggregate.Should().NotBeNull();
        }

        [Fact]
        public async void GetByIdAsync_WithNotFoundEvents_ShouldReturnNull()
        {
            var emptyEventList = new List<EventData>();

            _databaseProvider.QueryAsync<EventData>(Arg.Any<string>()).ReturnsForAnyArgs(emptyEventList);

            var aggregate = await _eventRepository.GetByIdAsync<Investor>(_aggregateId);

            aggregate.Should().BeNull();
        }

        
    }
}
