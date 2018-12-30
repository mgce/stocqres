using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Stocqres.Core.Authentication;
using Stocqres.Core.Exceptions;
using Stocqres.Identity.Domain;
using Stocqres.Identity.Repositories;

namespace Stocqres.Identity.Application.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly IUserRepository _userRepository;
        private readonly IPasswordHasher<User> _passwordHasher;
        private readonly ITokenService _tokenService;
        private readonly IRefreshTokenService _refreshTokenService;

        public AuthenticationService(IUserRepository userRepository, IPasswordHasher<User> passwordHasher,
            ITokenService tokenService, IRefreshTokenService refreshTokenService)
        {
            _userRepository = userRepository;
            _passwordHasher = passwordHasher;
            _tokenService = tokenService;
            _refreshTokenService = refreshTokenService;
        }

        public async Task<JsonWebToken> SignInAsync(string username, string password)
        {
            var user = await _userRepository.FindAsync(x => x.Username == username);
            if (user == null)
                throw new StocqresException("UserCodes does not exist.");

            var passwordVerification = _passwordHasher.VerifyHashedPassword(user, user.Password, password);
            if (passwordVerification == PasswordVerificationResult.Failed)
                throw new StocqresException("Username or password is incorrect.");

            var token = await _tokenService.CreateTokenForUser(user.Id);
            var refreshToken = await _refreshTokenService.CreateAsync(user.Id);
            token.RefreshToken = refreshToken.Token;

            return token;
        }
    }
}
