using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AutoFixture;
using Stocqres.Core.Domain;
using Stocqres.Core.Exceptions;
using Stocqres.Customers.Api.Wallet.Events;
using Stocqres.Customers.Investors.Domain;
using Stocqres.Customers.Wallet.Domain;
using Stocqres.SharedKernel.Enums;
using Stocqres.SharedKernel.Events;
using Xunit;

namespace Stocqres.UnitTests.Aggregates
{
    public class WalletUnitTests : AggregateBaseTest
    {
        private readonly Wallet _wallet;

        public WalletUnitTests()
        {
            _wallet = CreateWallet();
        }

        [Fact]
        public void Wallet_IsAggregateRoot()
        {
            Assert.IsAssignableFrom<AggregateRoot>(_wallet);
        }

        [Fact]
        public void Wallet_CurrentlyCreated_ShouldProduceWalletCreatedEvent()
        {
            var createdEvent = _wallet.GetUncommitedEvents().FirstOrDefault();

            Assert.NotNull(createdEvent);
            AssertProducedEvent<WalletCreatedEvent>(_wallet);
        }

        [Fact]
        public void Wallet_WithValidData_ShouldBeCharged()
        {
            var orderId = Guid.NewGuid();
            var actualAmount = _wallet.Amount;

            _wallet.ChargeWallet(orderId, actualAmount/2);

            Assert.Equal(actualAmount / 2, _wallet.Amount);
            AssertProducedEvent<WalletChargedEvent>(_wallet);
        }

        [Fact]
        public void Wallet_WithInvalidAmountToCharge_ShouldThrowException()
        {
            var orderId = Guid.NewGuid();
            var actualAmount = _wallet.Amount;

            Assert.Throws<StocqresException>(() => _wallet.ChargeWallet(orderId, actualAmount * 2));
        }

        [Fact]
        public void Wallet_WithAmountEqualToZero_ShouldNotBeCharged()
        {
            var orderId = Guid.NewGuid();

            Assert.Throws<StocqresException>(() => _wallet.ChargeWallet(orderId, 0));
        }

        [Fact]
        public void Wallet_WithValidData_ShouldAddNewStock()
        {
            var companyId = Guid.NewGuid();
            var orderId = Guid.NewGuid();
            var stockName = _fixture.Create<string>();
            var stockCode = _fixture.Create<string>();
            var unit = _fixture.Create<int>();
            var quantity = _fixture.Create<int>();

            _wallet.AddStock(orderId, companyId, stockName, stockCode, unit, quantity);

            Assert.NotNull(_wallet.StockList);
            Assert.Single(_wallet.StockList);
            AssertProducedEvent<StockToWalletAddedEvent>(_wallet);
        }

        [Fact]
        public void Wallet_WithUnitLowerThanZero_ShouldNotAddStock()
        {
            var companyId = Guid.NewGuid();
            var orderId = Guid.NewGuid();
            var stockName = _fixture.Create<string>();
            var stockCode = _fixture.Create<string>();
            var unit = 0;
            var quantity = _fixture.Create<int>();

            Assert.Throws<StocqresException>(() => _wallet.AddStock(orderId, companyId, stockName, stockCode, unit, quantity));
            AssertThatEventIsNotCreated<StockToWalletAddedEvent>(_wallet);
        }

        [Fact]
        public void Wallet_WithQuantityLowerThanZero_ShouldNotAddStock()
        {
            var companyId = Guid.NewGuid();
            var orderId = Guid.NewGuid();
            var stockName = _fixture.Create<string>();
            var stockCode = _fixture.Create<string>();
            var unit = _fixture.Create<int>();
            var quantity = 0;

            Assert.Throws<StocqresException>(() => _wallet.AddStock(orderId, companyId, stockName, stockCode, unit, quantity));
            AssertThatEventIsNotCreated<StockToWalletAddedEvent>(_wallet);
        }

        [Fact]
        public void Wallet_WithEmptyStockName_ShouldNotAddStock()
        {
            var companyId = Guid.NewGuid();
            var orderId = Guid.NewGuid();
            var stockName = "";
            var stockCode = _fixture.Create<string>();
            var unit = _fixture.Create<int>();
            var quantity = _fixture.Create<int>();

            Assert.Throws<StocqresException>(() => _wallet.AddStock(orderId, companyId, stockName, stockCode, unit, quantity));
            AssertThatEventIsNotCreated<StockToWalletAddedEvent>(_wallet);
        }

        [Fact]
        public void Wallet_WithEmptyStockCode_ShouldNotAddStock()
        {
            var companyId = Guid.NewGuid();
            var orderId = Guid.NewGuid();
            var stockName = _fixture.Create<string>();
            var stockCode = ""; ;
            var unit = _fixture.Create<int>();
            var quantity = _fixture.Create<int>();

            Assert.Throws<StocqresException>(() => _wallet.AddStock(orderId, companyId, stockName, stockCode, unit, quantity));
            AssertThatEventIsNotCreated<StockToWalletAddedEvent>(_wallet);
        }

        [Fact]
        public void Wallet_WithValidData_ShouldRollbackAmount()
        {
            var currentAmount = _wallet.Amount;
            var orderId = Guid.NewGuid();
            var amountToRollback = _fixture.Create<decimal>();

            _wallet.RollbackCharge(orderId, amountToRollback);

            Assert.Equal(currentAmount + amountToRollback, _wallet.Amount);
            AssertProducedEvent<WalletChargeRollbackedEvent>(_wallet);
        }

        [Fact]
        public void Wallet_WithAmountEqualToZero_ShouldNotBeRollbacked()
        {
            var orderId = Guid.NewGuid();
            var amountToRollback = 0;

            Assert.Throws<StocqresException>(() => _wallet.RollbackCharge(orderId, amountToRollback));
            AssertThatEventIsNotCreated<WalletChargeRollbackedEvent>(_wallet);
        }

        private Wallet CreateWallet()
        {
            var currency = _fixture.Create<Currency>();
            var amount = _fixture.Create<decimal>();

            return new Wallet(Guid.NewGuid(), currency, amount);
        }

        
    }
}
