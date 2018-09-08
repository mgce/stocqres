﻿using System.Threading.Tasks;
using Stocqres.Core.Authentication;
using Stocqres.Domain.Enums;

namespace Stocqres.Application.Token.Services
{
    public interface ITokenService
    {
        Task<JsonWebToken> SignInAsync(string username, string password, Role role = Role.Customer);
    }
}