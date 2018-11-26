using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AutoFixture;
using Stocqres.Core.Domain;
using Xunit;

namespace Stocqres.UnitTests.Aggregates
{
    public class AggregateBaseTest
    {
        protected readonly IFixture _fixture;

        protected AggregateBaseTest()
        {
            _fixture = new Fixture();
        }

        protected void AssertProducedEvent<TEvent>(AggregateRoot wallet)
        {
            var createdEvent = wallet.GetUncommitedEvents().Last();
            Assert.Equal(typeof(TEvent), createdEvent.GetType());
        }

        protected void AssertThatEventIsNotCreated<TEvent>(AggregateRoot wallet)
        {
            var createdEvent = wallet.GetUncommitedEvents().Last();
            Assert.NotEqual(typeof(TEvent), createdEvent.GetType());
        }
    }
}
