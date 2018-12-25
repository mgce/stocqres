using System;
using System.Collections.Generic;
using System.Text;
using AutoFixture;
using Stocqres.Core.Domain;
using Stocqres.Core.Exceptions;
using Stocqres.Customers.Api.Investors.Events;
using Stocqres.Customers.Investors.Domain;
using Xunit;

namespace Stocqres.UnitTests.Aggregates
{
    public class InvestorUnitTests : AggregateBaseTest
    {
        private readonly Investor _investor;

        public InvestorUnitTests()
        {
            _investor = CreateInvestor();
        }

        [Fact]
        public void Wallet_IsAggregateRoot()
        {
            Assert.IsAssignableFrom<AggregateRoot>(_investor);
        }

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
            AssertProducedEvent<InvestorCreatedEvent>(_investor);
        }

        [Fact]
        public void Investor_WithValidData_ShouldAssignWallet()
        {
            var walletId = Guid.NewGuid();

            _investor.AssignWallet(walletId);

            Assert.NotEqual(Guid.Empty, _investor.WalletId);
            Assert.Equal(walletId, _investor.WalletId);
            AssertProducedEvent<WalletToInvestorAssignedEvent>(_investor);
        }

        [Fact]
        public void Investor_WithEmptyWalletId_ShouldNotAssignWallet()
        {
            var walletId = Guid.Empty;

            Assert.Throws<StocqresException>(() => _investor.AssignWallet(walletId));
            AssertThatEventIsNotCreated<WalletToInvestorAssignedEvent>(_investor);
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
