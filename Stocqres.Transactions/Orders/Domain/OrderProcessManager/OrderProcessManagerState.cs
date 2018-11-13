using System;
using System.Collections.Generic;
using System.Text;

namespace Stocqres.Transactions.Orders.Domain.OrderProcessManager
{
    public enum OrderProcessManagerState
    {
        NotStarted,
        OrderPlaced,
        InvestorWalletCharged,
        CompanyCharged,
        StockAddedToWallet,
        OrderCompleted,
        CompanyChargeFailed,
        WalletChargeRollbacked,
        OrderFailed
    }
}
