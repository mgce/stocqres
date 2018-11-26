using System;
using System.Collections.Generic;
using System.Text;
using AutoFixture;
using Stocqres.Core.Exceptions;
using Stocqres.Customers.Investors.Domain;
using Stocqres.Customers.Investors.Domain.Events;
using Xunit;

namespace Stocqres.UnitTests.Aggregates
{
    public class InvestorUnitTests : AggregateBaseTest
    {
        [Fact]
        public void Investor_WithEmptyFirstName_ShouldNotBeCreated()
        {
            var userId = Guid.NewGuid();
            var firstName = "";
            var lastName = _fixture.Create<string>();

            Assert.Throws<StocqresException>(() => new Investor(userId, firstName, lastName));

        }

        [Fact]
        public void Investor_WithEmptyLastName_ShouldNotBeCreated()
        {
            var userId = Guid.NewGuid();
            var firstName = _fixture.Create<string>();
            var lastName = "";

            Assert.Throws<StocqresException>(() => new Investor(userId, firstName, lastName));

        }

        [Fact]
        public void Investor_WithEmptyUserId_ShouldNotBeCreated()
        {
            var userId = Guid.Empty;
            var firstName = _fixture.Create<string>();
            var lastName = _fixture.Create<string>();

            Assert.Throws<StocqresException>(() => new Investor(userId, firstName, lastName));

        }

        [Fact]
        public void Investor_CurrentlyCreated_ShouldProduceEvent()
        {
            var investor = CreateInvestor();
            AssertProducedEvent<InvestorCreatedEvent>(investor);
        }

        [Fact]
        public void Investor_WithValidData_ShouldAssignWallet()
        {
            var investor = CreateInvestor();
            var walletId = Guid.NewGuid();

            investor.AssignWallet(walletId);

            Assert.NotEqual(Guid.Empty, investor.WalletId);
            Assert.Equal(walletId, investor.WalletId);
            AssertProducedEvent<WalletToInvestorAssignedEvent>(investor);
        }

        [Fact]
        public void Investor_WithEmptyWalletId_ShouldNotAssignWallet()
        {
            var investor = CreateInvestor();
            var walletId = Guid.Empty;

            Assert.Throws<StocqresException>(() => investor.AssignWallet(walletId));
            AssertThatEventIsNotCreated<WalletToInvestorAssignedEvent>(investor);
        }

        private Investor CreateInvestor()
        {
            var userId = Guid.NewGuid();
            var firstName = _fixture.Create<string>();
            var lastName = _fixture.Create<string>();

            return new Investor(userId, firstName, lastName);
        }
    }
}
