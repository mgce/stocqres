using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Stocqres.Core.Authentication;
using Stocqres.Core.Exceptions;
using Stocqres.Domain.Enums;
using Stocqres.Identity.Domain;
using Stocqres.Identity.Repositories;

namespace Stocqres.Identity.Application.Services
{
    public class TokenService : ITokenService
    {
        private readonly IUserRepository _userRepository;
        private readonly IPasswordHasher<Domain.User> _passwordHasher;
        private readonly IJwtHandler _jwtHandler;
        private readonly IRefreshTokenRepository _refreshTokenRepository;

        public TokenService(IUserRepository userRepository, 
            IPasswordHasher<Domain.User> passwordHasher, 
            IJwtHandler jwtHandler, 
            IRefreshTokenRepository refreshTokenRepository)
        {
            _userRepository = userRepository;
            _passwordHasher = passwordHasher;
            _jwtHandler = jwtHandler;
            _refreshTokenRepository = refreshTokenRepository;
        }

        public async Task<JsonWebToken> SignInAsync(string username, string password)
        {
            var user = await _userRepository.GetAsync(x => x.Username == username);
            if(user == null)
                throw new StocqresException("UserCodes does not exist.");

            var result = _passwordHasher.VerifyHashedPassword(user, user.Password, password);
            if (result == PasswordVerificationResult.Failed)
                throw new StocqresException("Username or password is incorrect.");

            var jwt = _jwtHandler.CreateToken(user.Id.ToString());

            var refreshToken = new RefreshToken(user, _passwordHasher);
            await _refreshTokenRepository.CreateAsync(refreshToken);
            jwt.RefreshToken = refreshToken.Token;

            return jwt;

        }
    }
}
