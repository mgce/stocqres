using System;
using System.Collections.Generic;
using System.Text;
using Stocqres.Core.Domain;
using Stocqres.Transactions.Orders.Domain.Enums;

namespace Stocqres.Transactions.Orders.Presentation
{
    public class OrderProjection : IProjection
    {
        public Guid Id { get; set; }
        public Guid WalletId { get; set; }
        public Guid CompanyId { get; set; }
        public int Quantity { get; set; }
        public OrderState State { get; set; }

        public OrderProjection(Guid id, Guid walletId, Guid companyId, int quantity, OrderState state)
        {
            Id = id;
            WalletId = walletId;
            CompanyId = companyId;
            Quantity = quantity;
            State = state;
        }

    }
}
