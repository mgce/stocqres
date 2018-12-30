using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Stocqres.Core.Authentication;
using Stocqres.Core.Exceptions;
using Stocqres.Identity.Domain;
using Stocqres.Identity.Repositories;

namespace Stocqres.Identity.Application.Services
{
    public class RefreshTokenService : IRefreshTokenService
    {
        private readonly IUserRepository _userRepository;
        private readonly IRefreshTokenRepository _refreshTokenRepository;
        private readonly IPasswordHasher<Domain.User> _passwordHasher;
        private readonly IJwtHandler _jwtHandler;
        private readonly ITokenService _tokenService;

        public RefreshTokenService(IUserRepository userRepository,
            IRefreshTokenRepository refreshTokenRepository,
            IPasswordHasher<User> passwordHasher, IJwtHandler jwtHandler, ITokenService tokenService)
        {
            _userRepository = userRepository;
            _refreshTokenRepository = refreshTokenRepository;
            _passwordHasher = passwordHasher;
            _jwtHandler = jwtHandler;
            _tokenService = tokenService;
        }

        public async Task<RefreshToken> CreateAsync(Guid userId)
        {
            var user = await _userRepository.GetUserAsync(userId);
            if (user == null)
            {
                throw new StocqresException("UserCodes does not exist.");
            }

            var refreshToken = new RefreshToken(user, _passwordHasher);

            await _refreshTokenRepository.CreateAsync(refreshToken);
            await _refreshTokenRepository.SaveAsync();

            return refreshToken;
        }

        public async Task<JsonWebToken> CreateAccessTokenAsync(string token)
        {
            var refreshToken = await _refreshTokenRepository.FindAsync(x => x.Token == token);
            if (refreshToken == null)
            {
                throw new StocqresException("Refresh token was not found");
            }

            if (refreshToken.Revoked)
            {
                throw new StocqresException($"Refresh token: {refreshToken.Id} was revoked.");
            }

            var jwt = await _tokenService.CreateTokenForUser(refreshToken.UserId);

            jwt.RefreshToken = refreshToken.Token;

            return jwt;
        }

        public async Task RevokeAsync(string token, Guid userId)
        {
            var refreshToken = await _refreshTokenRepository.FindAsync(x => x.Token == token);
            if (refreshToken == null || refreshToken.UserId != userId)
            {
                throw new StocqresException("Refresh token was not found.");
            }

            refreshToken.Revoke();
            _refreshTokenRepository.Update(refreshToken);
        }
    }
}
