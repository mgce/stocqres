using System;
using System.Collections;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace Stocqres.Core.Authentication
{
    public class JwtHandler : IJwtHandler
    {
        private readonly JwtOptions _options;

        private static readonly ISet<string> DefaultClaims = new HashSet<string>
        {
            JwtRegisteredClaimNames.Sub,
            JwtRegisteredClaimNames.UniqueName,
            JwtRegisteredClaimNames.Jti,
            JwtRegisteredClaimNames.Iat,
            ClaimTypes.Role,
        };
        private readonly TokenValidationParameters _tokenValidationParameters;
        private readonly SigningCredentials _signingCredentials;
        public JwtHandler(JwtOptions options)
        {
            _options = options;
            var issuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_options.SecretKey));
            _signingCredentials = new SigningCredentials(issuerSigningKey, SecurityAlgorithms.HmacSha256);
            _tokenValidationParameters = new TokenValidationParameters
            {
                IssuerSigningKey =
                    new SymmetricSecurityKey(Encoding.UTF8.GetBytes(options.SecretKey)),
                ValidAudience = options.ValidAudience,
                ValidIssuer = options.Issuer,
                ValidateAudience = options.ValidateAudience,
                ValidateLifetime = options.ValidateLifetime
            };
        }

        public JsonWebToken CreateToken(string userId, string role = null, IDictionary<string, string> claims = null)
        {
            if (string.IsNullOrEmpty(userId))
            {
                throw new ArgumentException("User AggregateId claim cannot be empty", nameof(userId));
            }

            var now = DateTime.Now;
            var jwtClaims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, userId),
                new Claim(JwtRegisteredClaimNames.UniqueName, userId),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Iat, ToTimeStamp(now).ToString()),
            };
            if (!string.IsNullOrEmpty(role))
            {
                jwtClaims.Add(new Claim(ClaimTypes.Role, role));    
            }
            jwtClaims.AddRange(claims?.Select(claim => new Claim(claim.Key, claim.Value))
                               ?? Enumerable.Empty<Claim>());
            var expires = now.AddMinutes(_options.ExpiryMinutes);
            var jwt = new JwtSecurityToken(
                issuer: _options.Issuer,
                claims: jwtClaims,
                notBefore: now,
                expires: expires,
                signingCredentials: _signingCredentials
            );
            var token = new JwtSecurityTokenHandler().WriteToken(jwt);

            return new JsonWebToken
            {
                AccessToken = token,
                Expires = ToTimeStamp(expires)
            };
        }

        private Int32 ToTimeStamp(DateTime dateTime)
        {
            return (Int32) (dateTime.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;
        }
    }
}
