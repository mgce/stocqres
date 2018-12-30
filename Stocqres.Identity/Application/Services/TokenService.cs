using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Stocqres.Core.Authentication;
using Stocqres.Core.Exceptions;
using Stocqres.Core.Queries;
using Stocqres.Customers.Api.Investors.Presentation.Queries;
using Stocqres.Customers.Api.Investors.Presentation.Results;
using Stocqres.Identity.Domain;
using Stocqres.Identity.Repositories;

namespace Stocqres.Identity.Application.Services
{
    public class TokenService : ITokenService
    {
        private readonly IJwtHandler _jwtHandler;
        private readonly IQueryBus _queryBus;

        public TokenService(IJwtHandler jwtHandler,
            IQueryBus queryBus)
        {
            _jwtHandler = jwtHandler;
            _queryBus = queryBus;
        }

        public async Task<JsonWebToken> CreateTokenForUser(Guid userId)
        {
            var investor =
                _queryBus.QueryAsync<GetInvestorByUserIdQuery, GetInvestorByUserIdResult>(
                    new GetInvestorByUserIdQuery(userId));

            var claimsDictionary = new Dictionary<string, string>();

            var investorResult = await investor;

            if (investorResult != null)
            {
                claimsDictionary.Add("investorId", investorResult.InvestorId.ToString());
                claimsDictionary.Add("walletId", investorResult.WalletId.ToString());
            }

            return _jwtHandler.CreateToken(userId.ToString(), claimsDictionary);
        }
    }
}
