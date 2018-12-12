using System;
using System.Collections.Generic;
using System.Text;
using Stocqres.Transactions.Orders.Domain.Enums;

namespace Stocqres.Transactions.Orders.Domain.Order.Factories
{
    public class OrderFactory : IOrderFactory
    {
        public BuyOrder CreateBuyOrder(Guid walletId, Guid companyId, int quantity)
        {
            return new BuyOrder(walletId, companyId, quantity);
        }

        public SellOrder CreateSellOrder(Guid walletId, Guid companyId, int quantity, string stockCode)
        {
            return new SellOrder(walletId, companyId, quantity, stockCode);
        }
    }
}
