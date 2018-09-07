using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Stocqres.Core.Authentication;
using Stocqres.Core.Exceptions;
using Stocqres.Domain.Enums;
using Stocqres.Infrastructure;

namespace Stocqres.Application.Token.Services
{
    public class TokenService : ITokenService
    {
        private readonly IUserRepository _userRepository;
        private readonly IPasswordHasher<Domain.User> _passwordHasher;
        private readonly IJwtHandler _jwtHandler;

        public TokenService(IUserRepository userRepository, IPasswordHasher<Domain.User> passwordHasher, IJwtHandler jwtHandler)
        {
            _userRepository = userRepository;
            _passwordHasher = passwordHasher;
            _jwtHandler = jwtHandler;
        }

        public async Task<JsonWebToken> SignInAsync(string username, string password, Role role = Role.Customer)
        {
            var user = await _userRepository.GetAsync(x => x.Username == username);
            if(user == null)
                throw new StocqresException("User does not exist.");

            var result = _passwordHasher.VerifyHashedPassword(user, user.Password, password);
            if (result == PasswordVerificationResult.Failed)
                throw new StocqresException("Username or password is incorrect.");

            return _jwtHandler.CreateToken(user.Id.ToString(), user.Role.ToString());
        }
    }
}
