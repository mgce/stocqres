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
using Stocqres.Core.EventSourcing;
using Stocqres.Core.Exceptions;
using Stocqres.Customers.Investors.Domain;
using Stocqres.Customers.Investors.Domain.Events;
using Stocqres.Infrastructure.DatabaseProvider;
using Stocqres.Infrastructure.EventRepository;
using Stocqres.Infrastructure.EventRepository.Scripts;
using Stocqres.Infrastructure.EventStore;
using Stocqres.Infrastructure.Snapshots;
using Stocqres.Infrastructure.UnitOfWork;
using Xunit;

namespace Stocqres.UnitTests.EventSourcing
{
    public class EventReposiotryUnitTests : EventSourcingUnitTestsBase
    {
        private readonly IAggregateRootFactory _aggregateRootFactory;
        private readonly IEventBus _eventBus;
        private readonly IEventStore _eventStore;
        private readonly ISnapshotService _snapshotService;
        private readonly IEventRepository _eventRepository;

        public EventReposiotryUnitTests()
        {
            _aggregateRootFactory = Substitute.For<IAggregateRootFactory>();
            _eventBus = Substitute.For<IEventBus>();
            _eventStore = Substitute.For<IEventStore>();
            _snapshotService = Substitute.For<ISnapshotService>();
            _eventRepository = new EventRepository(_aggregateRootFactory, _eventBus, _eventStore, _snapshotService);
        }

        [Fact]
        public async void GetByIdAsync_WithExistingInvestor_ShouldReturnInvestorAggregate()
        {
            var eventToReturn = GetEvents();

            _eventStore.GetFromAsync<Investor>(_aggregateId).ReturnsForAnyArgs(eventToReturn);

            Snapshot snapshot = null;

            _snapshotService.GetLastAsync(Arg.Any<Guid>()).ReturnsForAnyArgs(snapshot);

            _aggregateRootFactory
                .CreateAsync<Investor>(eventToReturn)
                .Returns(_investor);

            var aggregate = await _eventRepository.GetByIdAsync<Investor>(_aggregateId);

            aggregate.Should().Be(_investor);
            aggregate.Should().NotBeNull();
            _aggregateRootFactory.Received().CreateAsync<Investor>(eventToReturn);
        }

        [Fact]
        public void GetByIdAsync_WithNotFoundEvents_ShouldThrowException()
        {
            var emptyEventList = new List<IEvent>();

            Snapshot snapshot = null;

            _snapshotService.GetLastAsync(Arg.Any<Guid>()).ReturnsForAnyArgs(snapshot);

            _eventStore.GetFromAsync<Investor>(_aggregateId).ReturnsForAnyArgs(emptyEventList);

            Assert.ThrowsAsync<StocqresException>(() => _eventRepository.GetByIdAsync<Investor>(_aggregateId));
        }

        [Fact]
        public void GetByIdAsync_WithEmptyGuid_ShouldThrowException() => 
            Assert.ThrowsAsync<StocqresException>(() => _eventRepository.GetByIdAsync<Investor>(Guid.Empty));


    }
}
