using System;

namespace Stocqres.Transactions.Orders.Domain.Order.Factories
{
    public interface IOrderFactory
    {
        Order CreateBuyOrder(Guid walletId, Guid companyId, int quantity);
        Order CreateSellOrder(Guid walletId, Guid companyId, int quantity);
    }
}