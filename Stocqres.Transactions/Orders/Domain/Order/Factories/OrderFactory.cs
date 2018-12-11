using System;
using System.Collections.Generic;
using System.Text;
using Stocqres.Transactions.Orders.Domain.Enums;

namespace Stocqres.Transactions.Orders.Domain.Order.Factories
{
    public class OrderFactory : IOrderFactory
    {
        public Order CreateBuyOrder(Guid walletId, Guid companyId, int quantity)
        {
            return new Order(walletId, companyId, quantity, OrderType.Buy);
        }

        public Order CreateSellOrder(Guid walletId, Guid companyId, int quantity)
        {
            return new Order(walletId, companyId, quantity, OrderType.Sell);
        }
    }
}
