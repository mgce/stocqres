using System;

namespace Stocqres.Transactions.Orders.Domain.Order.Factories
{
    public interface IOrderFactory
    {
        BuyOrder CreateBuyOrder(Guid walletId, Guid companyId, int quantity);
        SellOrder CreateSellOrder(Guid walletId, Guid companyId, int quantity, string stockCode);
    }
}