using System;
using Stocqres.Core.Commands;

namespace Stocqres.SharedKernel.Commands
{
    public class ChargeCompanyCommand : ICommand
    {
        public Guid CompanyId { get; set; }
        public int Quantity { get; set; }

        public ChargeCompanyCommand(Guid companyId, int quantity)
        {
            CompanyId = companyId;
            Quantity = quantity;
        }
    }
}
