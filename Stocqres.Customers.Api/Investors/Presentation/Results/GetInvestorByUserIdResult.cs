using System;
using System.Collections.Generic;
using System.Text;

namespace Stocqres.Customers.Api.Investors.Presentation.Results
{
    public class GetInvestorByUserIdResult
    {
        public Guid InvestorId { get; set; }
        public Guid WalletId { get; set; }

        public GetInvestorByUserIdResult(Guid investorId, Guid walletId)
        {
            InvestorId = investorId;
            WalletId = walletId;
        }
    }
}
