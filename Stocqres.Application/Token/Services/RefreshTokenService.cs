using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Stocqres.Infrastructure;

namespace Stocqres.Application.Token.Services
{
    public class RefreshTokenService
    {
        private readonly IUserRepository _userRepository;

        public RefreshTokenService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }
        public async Task CreateAsync(Guid userId)
        {
            var user = await _userRepository.GetAsync(u => u.Id == userId);
            //if()
        }
    }
}
