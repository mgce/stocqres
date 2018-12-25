using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Stocqres.Core.Queries;
using Stocqres.Customers.Api.Investors.Presentation;
using Stocqres.Customers.Api.Investors.Presentation.Queries;
using Stocqres.Customers.Api.Investors.Presentation.Results;
using Stocqres.Infrastructure.Projections;

namespace Stocqres.Customers.Investors.Presentation.Finders
{
    public class InvestorFinder : IQueryHandler<GetInvestorByUserIdQuery, GetInvestorByUserIdResult>
    {
        private readonly IProjectionReader _projectionReader;

        public InvestorFinder(IProjectionReader projectionReader)
        {
            _projectionReader = projectionReader;
        }
        public async Task<GetInvestorByUserIdResult> HandleAsync(GetInvestorByUserIdQuery query)
        {
            var investor = await _projectionReader.GetAsync<InvestorProjection>(i => i.UserId == query.UserId);
            return new GetInvestorByUserIdResult(investor.Id, investor.WalletId);
        }
    }
}
