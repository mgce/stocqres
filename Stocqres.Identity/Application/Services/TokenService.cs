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
        private readonly IUserRepository _userRepository;
        private readonly IPasswordHasher<User> _passwordHasher;
        private readonly IJwtHandler _jwtHandler;
        private readonly IRefreshTokenRepository _refreshTokenRepository;
        private readonly IQueryBus _queryBus;

        public TokenService(IUserRepository userRepository, 
            IPasswordHasher<User> passwordHasher, 
            IJwtHandler jwtHandler, 
            IRefreshTokenRepository refreshTokenRepository,
            IQueryBus queryBus)
        {
            _userRepository = userRepository;
            _passwordHasher = passwordHasher;
            _jwtHandler = jwtHandler;
            _refreshTokenRepository = refreshTokenRepository;
            _queryBus = queryBus;
        }

        public async Task<JsonWebToken> SignInAsync(string username, string password)
        {
            var user = await _userRepository.FindAsync(x => x.Username == username);
            if(user == null)
                throw new StocqresException("UserCodes does not exist.");

            var investor =
                await _queryBus.Send<GetInvestorByUserIdQuery, GetInvestorByUserIdResult>(
                    new GetInvestorByUserIdQuery(user.Id));

            var claimsDictionary = new Dictionary<string, string>();

            if (investor != null)
            {
                claimsDictionary.Add("investorId", investor.InvestorId.ToString());
                claimsDictionary.Add("walletId", investor.WalletId.ToString());
            }

            var result = _passwordHasher.VerifyHashedPassword(user, user.Password, password);
            if (result == PasswordVerificationResult.Failed)
                throw new StocqresException("Username or password is incorrect.");

            var jwt = _jwtHandler.CreateToken(user.Id.ToString(), claimsDictionary);

            var refreshToken = new RefreshToken(user, _passwordHasher);
            await _refreshTokenRepository.CreateAsync(refreshToken);
            jwt.RefreshToken = refreshToken.Token;

            return jwt;

        }
    }
}
