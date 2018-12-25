using System;
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
    [Table(nameof(BuyOrderProcessManager), Schema = "Transactions")]
    public class BuyOrderProcessManager : ProcessManager
    {
        public BuyOrderProcessManager(Guid aggregateId)
        {
            AggregateId = aggregateId;
            State = BuyOrderProcessManagerState.NotStarted;
        }

        public Guid WalletId { get; set; }
        public Guid CompanyId { get; set; }
        public string StockName { get; set; }
        public string StockCode { get; set; }
        public int StockUnit { get; set; }
        public int StockQuantity { get; set; }
        public decimal ChargedWalletAmount { get; set; }
        public string CancelReason { get; set; }
        public BuyOrderProcessManagerState State { get; set; }
        

        public void When(BuyOrderCreatedEvent message)
        {
            switch (State)
            {
                case BuyOrderProcessManagerState.NotStarted:
                        WalletId = message.WalletId;
                        CompanyId = message.CompanyId;
                        StockQuantity = message.Quantity;
                        State = BuyOrderProcessManagerState.OrderPlaced;
                        ProcessCommand(new ChargeWalletAmountCommand(message.WalletId, message.CompanyId, message.AggregateId, message.Quantity));
                    break;
                // idempotence - same message sent twice
                case BuyOrderProcessManagerState.OrderPlaced:
                    break;
                default:
                    throw new StocqresException("Invalid state for this message");
            }
        }

        public void When(WalletChargedEvent message)
        {
            switch (State)
            {
                case BuyOrderProcessManagerState.OrderPlaced:
                    State = BuyOrderProcessManagerState.InvestorWalletCharged;
                    ChargedWalletAmount = message.Amount;
                    ProcessCommand(new ChargeCompanyCommand(CompanyId, AggregateId, StockQuantity));
                    break;
                case BuyOrderProcessManagerState.InvestorWalletCharged:
                    break;
                default:
                    throw new StocqresException("Invalid state for this message");
            }
        }

        public void When(CompanyChargedEvent message)
        {
            switch (State)
            {
                case BuyOrderProcessManagerState.InvestorWalletCharged:
                    State = BuyOrderProcessManagerState.CompanyCharged;
                    StockName = message.StockName;
                    StockCode = message.StockCode;
                    StockQuantity = message.StockQuantity;
                    StockUnit = message.StockUnit;
                    ProcessCommand(new AddStockToWalletCommand(WalletId, AggregateId, CompanyId, message.StockName, message.StockCode, message.StockUnit, message.StockQuantity));
                    break;
                case BuyOrderProcessManagerState.StockAddedToWallet:
                    break;
                default:
                    throw new StocqresException("Invalid state for this message");
            }
        }

        public void When(StockToWalletAddedEvent message)
        {
            switch (State)
            {
                case BuyOrderProcessManagerState.CompanyCharged:
                    State = BuyOrderProcessManagerState.OrderCompleted;
                    ProcessCommand(new FinishBuyOrderCommand(AggregateId));
                    break;
                default:
                    throw new StocqresException("Invalid state for this message");
            }
        }

        public void When(CompanyChargeFailedEvent message)
        {
            switch (State)
            {
                case BuyOrderProcessManagerState.CompanyCharged:
                    State = BuyOrderProcessManagerState.CompanyChargeFailed;
                    CancelReason = message.CancelReason;
                    ProcessCommand(new RollbackWalletChargeCommand(WalletId, AggregateId, ChargedWalletAmount));
                    break;
                case BuyOrderProcessManagerState.CompanyChargeFailed:
                    break;
                default:
                    throw new StocqresException("Invalid state for this message");
            }
        }

        public void When(WalletChargeRollbackedEvent message)
        {
            switch (State)
            {
                case BuyOrderProcessManagerState.CompanyChargeFailed:
                    State = BuyOrderProcessManagerState.WalletChargeRollbacked;
                    ProcessCommand(new CancelOrderCommand(AggregateId, CancelReason));
                    break;
                case BuyOrderProcessManagerState.WalletChargeRollbacked:
                    break;
                default:
                    throw new StocqresException("Invalid state for this message");
            }
        }

        public void When(BuyOrderCancelledEvent message)
        {
            switch (State)
            {
                case BuyOrderProcessManagerState.WalletChargeRollbacked:
                    State = BuyOrderProcessManagerState.OrderFailed;
                    break;
                default:
                    throw new StocqresException("Invalid state for this message");
            }
        }
    }
}
