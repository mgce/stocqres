﻿using System;
using System.ComponentModel.DataAnnotations.Schema;
using Stocqres.Core.Exceptions;
using Stocqres.Customers.Api.Companies.Commands;
using Stocqres.Customers.Api.Wallet.Commands;
using Stocqres.Customers.Api.Wallet.Events;
using Stocqres.SharedKernel.Events;
using Stocqres.Transactions.Orders.Domain.Command;
using Stocqres.Transactions.Orders.Domain.Enums;
using Stocqres.Transactions.Orders.Domain.Events;

namespace Stocqres.Transactions.Orders.Domain.ProcessManagers
{
    [Table(nameof(SellOrderProcessManager), Schema = "Transactions")]
    public class SellOrderProcessManager : ProcessManager
    {
        public SellOrderProcessManager(Guid walletId, Guid companyId, string stockCode, int stockQuantity,
            string cancelReason, SellOrderProcessManagerState state)
        {
            WalletId = walletId;
            CompanyId = companyId;
            StockCode = stockCode;
            StockQuantity = stockQuantity;
            CancelReason = cancelReason;
            State = state;
        }

        public SellOrderProcessManager(Guid aggregateId)
        {
            AggregateId = aggregateId;
            State = SellOrderProcessManagerState.NotStarted;
        }

        public Guid WalletId { get; set; }
        public Guid CompanyId { get; set; }
        public string StockCode { get; set; }
        public int StockQuantity { get; set; }
        public string CancelReason { get; set; }
        public SellOrderProcessManagerState State { get; set; }

        public void When(SellOrderCreatedEvent message)
        {
            switch (State)
            {
                case SellOrderProcessManagerState.NotStarted:
                    WalletId = message.WalletId;
                    CompanyId = message.CompanyId;
                    StockQuantity = message.Quantity;
                    State = SellOrderProcessManagerState.OrderPlaced;
                    ProcessCommand(new TakeOffStocksFromWalletCommand(message.WalletId, message.CompanyId,
                        message.AggregateId, message.Quantity));
                    break;
                // idempotence - same message sent twice
                case SellOrderProcessManagerState.OrderPlaced:
                    break;
                default:
                    throw new StocqresException("Invalid state for this message");
            }
        }

        public void When(StocksTakedOffFromWalletEvent message)
        {
            switch (State)
            {
                case SellOrderProcessManagerState.OrderPlaced:
                    CompanyId = message.CompanyId;
                    StockQuantity = message.Quantity;
                    State = SellOrderProcessManagerState.StocksTakedOffFromWallet;
                    ProcessCommand(new AddStocksToCompanyCommand(CompanyId, message.AggregateId, message.Quantity,
                        StockCode));
                    break;
                // idempotence - same message sent twice
                case SellOrderProcessManagerState.StocksTakedOffFromWallet:
                    break;
                default:
                    throw new StocqresException("Invalid state for this message");
            }
        }

        public void When(StocksAddedToCompanyEvent message)
        {
            switch (State)
            {
                case SellOrderProcessManagerState.StocksTakedOffFromWallet:
                    State = SellOrderProcessManagerState.StocksAddedToCompany;
                    ProcessCommand(new TopUpWalletAmountCommand(WalletId, AggregateId, StockCode,
                        message.StockQuantity));
                    break;
                // idempotence - same message sent twice
                case SellOrderProcessManagerState.StocksAddedToCompany:
                    break;
                default:
                    throw new StocqresException("Invalid state for this message");
            }
        }

        public void When(WalletAmountToppedUpEvent message)
        {
            switch (State)
            {
                case SellOrderProcessManagerState.StocksTakedOffFromWallet:
                    State = SellOrderProcessManagerState.WalletToppedUp;
                    ProcessCommand(new FinishBuyOrderCommand(AggregateId));
                    break;
                // idempotence - same message sent twice
                case SellOrderProcessManagerState.StocksAddedToCompany:
                    break;
                default:
                    throw new StocqresException("Invalid state for this message");
            }
        }

        public void When(SellOrderCancelledEvent message)
        {
            State = SellOrderProcessManagerState.OrderFailed;
        }

        public void When(SellOrderFinishedEvent message)
        {
            switch (State)
            {
                case SellOrderProcessManagerState.WalletToppedUp:
                    State = SellOrderProcessManagerState.OrderFinished;
                    break;
                // idempotence - same message sent twice
                case SellOrderProcessManagerState.OrderFinished:
                    break;
                default:
                    throw new StocqresException("Invalid state for this message");
            }
        }

    }
}
