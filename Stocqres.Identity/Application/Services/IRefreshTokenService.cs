using System;
using System.Threading.Tasks;
using Stocqres.Core.Authentication;
using Stocqres.Identity.Domain;

namespace Stocqres.Identity.Application.Services
{
    public interface IRefreshTokenService
    {
        Task<RefreshToken> CreateAsync(Guid userId);
        Task<JsonWebToken> CreateAccessTokenAsync(string token);
        Task RevokeAsync(string token, Guid userId);
    }
}