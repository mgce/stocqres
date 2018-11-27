using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentAssertions;
using Stocqres.UnitTests.Helpers;
using Xunit;

namespace Stocqres.UnitTests.EventSourcing
{
    public class AggregateRootUnitTests : EventSourcingUnitTestsBase
    {
        [Fact]
        public void FakeAggregate_AfterCreating_ShouldHaveOneUncommitedEvent()
        {
            var aggregateId = Guid.NewGuid();

            var fakeAggregate = new FakeAggregateRoot(aggregateId);

            fakeAggregate.GetUncommitedEvents().Should().NotBeEmpty().And.HaveCount(1);
            fakeAggregate.GetUncommitedEvents().Single().Should().BeAssignableTo<FakeAggregateCreatedEvent>();
        }

        [Fact]
        public void FakeAggregate_AfterAppliedFakeMethod_ShouldIncreaseVersion()
        {
            var aggregateId = Guid.NewGuid();
            var fakeAggregate = new FakeAggregateRoot(aggregateId);
            var versionBeforeChange = fakeAggregate.Version;

            fakeAggregate.FakeMethod();

            Assert.Equal(versionBeforeChange, fakeAggregate.Version - 1);
            fakeAggregate.Version.Should().Be(2);
            fakeAggregate.GetUncommitedEvents().Should().NotBeEmpty().And.HaveCount(2);
        }

    }
}
