using System;
using System.Threading.Tasks;
using Stocqres.Core.Authentication;

namespace Stocqres.Identity.Application.Services
{
    public interface IRefreshTokenService
    {
        Task CreateAsync(Guid userId);
        Task<JsonWebToken> CreateAccessTokenAsync(string token);
        Task RevokeAsync(string token, Guid userId);
    }
}