using System.Threading.Tasks;
using Stocqres.Core.Authentication;

namespace Stocqres.Identity.Application.Services
{
    public interface ITokenService
    {
        Task<JsonWebToken> SignInAsync(string username, string password);
    }
}
