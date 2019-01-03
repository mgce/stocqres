using System;
using System.Collections.Generic;
using System.Text;
using Stocqres.Core.Queries;
using Stocqres.Customers.Api.Investors.Presentation.Results;

namespace Stocqres.Customers.Api.Investors.Presentation.Queries
{
    public class GetInvestorByUserIdQuery : IQuery<GetInvestorByUserIdResult>
    {
        public Guid UserId { get; set; }

        public GetInvestorByUserIdQuery(Guid userId)
        {
            UserId = userId;
        }
    }
}
