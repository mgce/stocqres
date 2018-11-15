using System;
using Stocqres.Core.Commands;

namespace Stocqres.SharedKernel.Commands
{
    public class ChargeCompanyCommand : ICommand
    {
        public Guid CompanyId { get; set; }
        public Guid OrderId { get; set; }
        public int Quantity { get; set; }

        public ChargeCompanyCommand(Guid companyId, Guid orderId, int quantity)
        {
            CompanyId = companyId;
            OrderId = orderId;
            Quantity = quantity;
        }
    }
}
