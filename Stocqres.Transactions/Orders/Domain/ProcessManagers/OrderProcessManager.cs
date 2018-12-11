using System;
using System.ComponentModel.DataAnnotations.Schema;
using Stocqres.Core.Exceptions;
using Stocqres.SharedKernel.Commands;
using Stocqres.SharedKernel.Events;
using Stocqres.Transactions.Orders.Domain.Command;
using Stocqres.Transactions.Orders.Domain.Events;

namespace Stocqres.Transactions.Orders.Domain.ProcessManagers
{
    [Table("OrderProcessManager", Schema = "Transactions")]
    public class OrderProcessManager : ProcessManager
    {
        public OrderProcessManager(Guid aggregateId)
        {
            AggregateId = aggregateId;
            State = OrderProcessManagerState.NotStarted;
        }

        public Guid WalletId { get; set; }
        public Guid CompanyId { get; set; }
        public string StockName { get; set; }
        public string StockCode { get; set; }
        public int StockUnit { get; set; }
        public int StockQuantity { get; set; }
        public decimal ChargedWalletAmount { get; set; }
        public string CancelReason { get; set; }
        public OrderProcessManagerState State { get; set; }
        

        public void When(BuyOrderCreatedEvent message)
        {
            switch (State)
            {
                case OrderProcessManagerState.NotStarted:
                        WalletId = message.WalletId;
                        CompanyId = message.CompanyId;
                        StockQuantity = message.Quantity;
                        State = OrderProcessManagerState.OrderPlaced;
                        ProcessCommand(new ChargeWalletCommand(message.WalletId, message.CompanyId, message.AggregateId, message.Quantity));
                    break;
                // idempotence - same message sent twice
                case OrderProcessManagerState.OrderPlaced:
                    break;
                default:
                    throw new StocqresException("Invalid state for this message");
            }
        }

        public void When(WalletChargedEvent message)
        {
            switch (State)
            {
                case OrderProcessManagerState.OrderPlaced:
                    State = OrderProcessManagerState.InvestorWalletCharged;
                    ChargedWalletAmount = message.Amount;
                    ProcessCommand(new ChargeCompanyCommand(CompanyId, AggregateId, StockQuantity));
                    break;
                case OrderProcessManagerState.InvestorWalletCharged:
                    break;
                default:
                    throw new StocqresException("Invalid state for this message");
            }
        }

        public void When(CompanyChargedEvent message)
        {
            switch (State)
            {
                case OrderProcessManagerState.InvestorWalletCharged:
                    State = OrderProcessManagerState.CompanyCharged;
                    StockName = message.StockName;
                    StockCode = message.StockCode;
                    StockQuantity = message.StockQuantity;
                    StockUnit = message.StockUnit;
                    ProcessCommand(new AddStockToWalletCommand(WalletId, AggregateId, message.StockName, message.StockCode, message.StockUnit, message.StockQuantity));
                    break;
                case OrderProcessManagerState.StockAddedToWallet:
                    break;
                default:
                    throw new StocqresException("Invalid state for this message");
            }
        }

        public void When(StockToWalletAddedEvent message)
        {
            switch (State)
            {
                case OrderProcessManagerState.CompanyCharged:
                    State = OrderProcessManagerState.OrderCompleted;
                    ProcessCommand(new FinishOrderCommand(AggregateId));
                    break;
                default:
                    throw new StocqresException("Invalid state for this message");
            }
        }

        public void When(CompanyChargeFailedEvent message)
        {
            switch (State)
            {
                case OrderProcessManagerState.CompanyCharged:
                    State = OrderProcessManagerState.CompanyChargeFailed;
                    CancelReason = message.CancelReason;
                    ProcessCommand(new RollbackWalletChargeCommand(WalletId, AggregateId, ChargedWalletAmount));
                    break;
                case OrderProcessManagerState.CompanyChargeFailed:
                    break;
                default:
                    throw new StocqresException("Invalid state for this message");
            }
        }

        public void When(WalletChargeRollbackedEvent message)
        {
            switch (State)
            {
                case OrderProcessManagerState.CompanyChargeFailed:
                    State = OrderProcessManagerState.WalletChargeRollbacked;
                    ProcessCommand(new CancelOrderCommand(AggregateId, CancelReason));
                    break;
                case OrderProcessManagerState.WalletChargeRollbacked:
                    break;
                default:
                    throw new StocqresException("Invalid state for this message");
            }
        }

        public void When(OrderCancelledEvent message)
        {
            switch (State)
            {
                case OrderProcessManagerState.WalletChargeRollbacked:
                    State = OrderProcessManagerState.OrderFailed;
                    break;
                default:
                    throw new StocqresException("Invalid state for this message");
            }
        }

    }
}
