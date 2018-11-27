using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentAssertions;
using NSubstitute.ExceptionExtensions;
using Stocqres.Core.Domain;
using Stocqres.Core.Events;
using Stocqres.Core.Exceptions;
using Stocqres.Customers.Investors.Domain;
using Stocqres.Infrastructure.EventRepository;
using Xunit;

namespace Stocqres.UnitTests.EventSourcing
{
    public class AggregateFactoryUnitTests : EventSourcingUnitTestsBase
    {
        private IAggregateRootFactory _aggregateRootFactory;
        public AggregateFactoryUnitTests()
        {
            _aggregateRootFactory = new AggregateRootFactory();
        }

        [Fact]
        public void Create_WithListOfEvents_ShouldReturnAggregate()
        {
            var events = GetEvents().OrderBy(e => e.Version).Select(x => x.DeserializeEvent());

            var aggregate = _aggregateRootFactory.CreateAsync<Investor>(events);

            aggregate.Should().NotBeNull();
            aggregate.Should().BeAssignableTo<Investor>();
        }

        [Fact]
        public void Create_WithNullListOfEvents_ShouldThrowException()
        {
            Action act = () => _aggregateRootFactory.CreateAsync<Investor>(null);

            act.Should().Throw<StocqresException>();
        }

        [Fact]
        public void Create_WithEmptyListOfEvents_ShouldReturnNull()
        {
            Action act = () => _aggregateRootFactory.CreateAsync<Investor>(new List<IEvent>());

            act.Should().Throw<StocqresException>();
        }
    }
}
