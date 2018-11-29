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
using Stocqres.Core.Exceptions;
using Stocqres.Customers.Investors.Domain;
using Stocqres.Customers.Investors.Domain.Events;
using Stocqres.Infrastructure.DatabaseProvider;
using Stocqres.Infrastructure.EventRepository;
using Stocqres.Infrastructure.EventRepository.Scripts;
using Stocqres.Infrastructure.UnitOfWork;
using Xunit;

namespace Stocqres.UnitTests.EventSourcing
{
    public class EventReposiotryUnitTests : EventSourcingUnitTestsBase
    {
        private readonly IAggregateRootFactory _aggregateRootFactory;
        private readonly IEventBus _eventBus;
        private readonly IDatabaseProvider _databaseProvider;
        private readonly IEventRepository _eventRepository;

        public EventReposiotryUnitTests()
        {
            _aggregateRootFactory = Substitute.For<IAggregateRootFactory>();
            _eventBus = Substitute.For<IEventBus>();
            _databaseProvider = Substitute.For<IDatabaseProvider>();
            _eventRepository = new EventRepository(_aggregateRootFactory, _eventBus, _databaseProvider);
        }

        [Fact]
        public async void GetByIdAsync_WithExistingInvestor_ShouldReturnInvestorAggregate()
        {
            var eventToReturn = GetEvents();

            var getAggregateSql = EventRepositoryScriptsAsStrings.GetAggregate(typeof(Investor).Name, _aggregateId);

            _databaseProvider.QueryAsync<EventData>(getAggregateSql, Arg.Any<object>()).Returns(eventToReturn);

            var deserializedEvents = GetEvents().OrderBy(e => e.Version).Select(x => x.DeserializeEvent());

            _aggregateRootFactory
                .CreateAsync<Investor>(deserializedEvents)
                .Returns(_investor);

            var aggregate = await _eventRepository.GetByIdAsync<Investor>(_aggregateId);

            aggregate.Should().Be(_investor);
            aggregate.Should().NotBeNull();
            _databaseProvider.Received().QueryAsync<EventData>(getAggregateSql, Arg.Any<object>());
            _aggregateRootFactory.Received().CreateAsync<Investor>(deserializedEvents);
        }

        [Fact]
        public void GetByIdAsync_WithNotFoundEvents_ShouldThrowException()
        {
            var emptyEventList = new List<EventData>();

            _databaseProvider.QueryAsync<EventData>(Arg.Any<string>()).ReturnsForAnyArgs(emptyEventList);

            Action act = () => _eventRepository.GetByIdAsync<Investor>(_aggregateId);

            act.Should().Throw<StocqresException>();
        }

        [Fact]
        public void GetByIdAsync_WithEmptyGuid_ShouldThrowException()
        {
            Action act = () => _eventRepository.GetByIdAsync<Investor>(Guid.Empty);

            Assert.Throws<StocqresException>(act);
        }


    }
}
