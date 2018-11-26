using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AutoFixture;
using Stocqres.Core.Exceptions;
using Stocqres.Customers.Investors.Domain;
using Stocqres.Customers.Wallet.Domain;
using Stocqres.Customers.Wallet.Events;
using Stocqres.SharedKernel.Events;
using Xunit;

namespace Stocqres.UnitTests.Aggregates
{
    public class WalletUnitTests : AggregateBaseTest
    {
        [Fact]
        public void Wallet_CurrentlyCreated_ShouldProduceWalletCreatedEvent()
        {
            var wallet = CreateWallet();
            var createdEvent = wallet.GetUncommitedEvents().FirstOrDefault();

            Assert.NotNull(createdEvent);
            AssertProducedEvent<WalletCreatedEvent>(wallet);
        }

        [Fact]
        public void Wallet_WithValidData_ShouldBeCharged()
        {
            var wallet = CreateWallet();
            var orderId = Guid.NewGuid();
            var actualAmount = wallet.Amount;
            
            wallet.ChargeWallet(orderId, actualAmount/2);

            Assert.Equal(actualAmount / 2, wallet.Amount);
            AssertProducedEvent<WalletChargedEvent>(wallet);
        }

        [Fact]
        public void Wallet_WithInvalidAmountToCharge_ShouldThrowException()
        {
            var wallet = CreateWallet();
            var orderId = Guid.NewGuid();
            var actualAmount = wallet.Amount;

            Assert.Throws<StocqresException>(() => wallet.ChargeWallet(orderId, actualAmount * 2));
        }

        [Fact]
        public void Wallet_WithAmountEqualToZero_ShouldNotBeCharged()
        {
            var wallet = CreateWallet();
            var orderId = Guid.NewGuid();

            Assert.Throws<StocqresException>(() => wallet.ChargeWallet(orderId, 0));
        }

        [Fact]
        public void Wallet_WithValidData_ShouldAddNewStock()
        {
            var wallet = CreateWallet();
            var orderId = Guid.NewGuid();
            var stockName = _fixture.Create<string>();
            var stockCode = _fixture.Create<string>();
            var unit = _fixture.Create<int>();
            var quantity = _fixture.Create<int>();

            wallet.AddStock(orderId, stockName, stockCode, unit, quantity);

            Assert.NotNull(wallet.StockList);
            Assert.Single(wallet.StockList);
            AssertProducedEvent<StockToWalletAddedEvent>(wallet);
        }

        [Fact]
        public void Wallet_WithUnitLowerThanZero_ShouldNotAddStock()
        {
            var wallet = CreateWallet();
            var orderId = Guid.NewGuid();
            var stockName = _fixture.Create<string>();
            var stockCode = _fixture.Create<string>();
            var unit = 0;
            var quantity = _fixture.Create<int>();

            Assert.Throws<StocqresException>(() => wallet.AddStock(orderId, stockName, stockCode, unit, quantity));
            AssertThatEventIsNotCreated<StockToWalletAddedEvent>(wallet);
        }

        [Fact]
        public void Wallet_WithQuantityLowerThanZero_ShouldNotAddStock()
        {
            var wallet = CreateWallet();
            var orderId = Guid.NewGuid();
            var stockName = _fixture.Create<string>();
            var stockCode = _fixture.Create<string>();
            var unit = _fixture.Create<int>();
            var quantity = 0;

            Assert.Throws<StocqresException>(() => wallet.AddStock(orderId, stockName, stockCode, unit, quantity));
            AssertThatEventIsNotCreated<StockToWalletAddedEvent>(wallet);
        }

        [Fact]
        public void Wallet_WithEmptyStockName_ShouldNotAddStock()
        {
            var wallet = CreateWallet();
            var orderId = Guid.NewGuid();
            var stockName = "";
            var stockCode = _fixture.Create<string>();
            var unit = _fixture.Create<int>();
            var quantity = _fixture.Create<int>();

            Assert.Throws<StocqresException>(() => wallet.AddStock(orderId, stockName, stockCode, unit, quantity));
            AssertThatEventIsNotCreated<StockToWalletAddedEvent>(wallet);
        }

        [Fact]
        public void Wallet_WithEmptyStockCode_ShouldNotAddStock()
        {
            var wallet = CreateWallet();
            var orderId = Guid.NewGuid();
            var stockName = _fixture.Create<string>();
            var stockCode = ""; ;
            var unit = _fixture.Create<int>();
            var quantity = _fixture.Create<int>();

            Assert.Throws<StocqresException>(() => wallet.AddStock(orderId, stockName, stockCode, unit, quantity));
            AssertThatEventIsNotCreated<StockToWalletAddedEvent>(wallet);
        }

        [Fact]
        public void Wallet_WithValidData_ShouldRollbackAmount()
        {
            var wallet = CreateWallet();
            var currentAmount = wallet.Amount;
            var orderId = Guid.NewGuid();
            var amountToRollback = _fixture.Create<decimal>();

            wallet.RollbackCharge(orderId, amountToRollback);

            Assert.Equal(currentAmount + amountToRollback, wallet.Amount);
            AssertProducedEvent<WalletChargeRollbackedEvent>(wallet);
        }

        [Fact]
        public void Wallet_WithAmountEqualToZero_ShouldNotBeRollbacked()
        {
            var wallet = CreateWallet();
            var currentAmount = wallet.Amount;
            var orderId = Guid.NewGuid();
            var amountToRollback = 0;

            Assert.Throws<StocqresException>(() => wallet.RollbackCharge(orderId, amountToRollback));
            AssertThatEventIsNotCreated<WalletChargeRollbackedEvent>(wallet);
        }

        private Wallet CreateWallet()
        {
            var currency = _fixture.Create<Currency>();
            var amount = _fixture.Create<decimal>();

            return new Wallet(Guid.NewGuid(), currency, amount);
        }

        
    }
}
