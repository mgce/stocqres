using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using Autofac;
using Microsoft.AspNetCore.Identity;
using Stocqres.Core.Commands;
using Stocqres.Core.Events;
using Stocqres.Identity.Application.Services;
using Stocqres.Identity.Domain;

namespace Stocqres.Identity
{
    public static class IdentityDependencyContainer
    {
        public static void Load(ContainerBuilder builder)
        {
            builder.RegisterType<TokenService>().As<ITokenService>();
            builder.RegisterType<RefreshTokenService>().As<IRefreshTokenService>();
            builder.RegisterType<PasswordHasher<User>>().As<IPasswordHasher<User>>();
        }
    }
}
