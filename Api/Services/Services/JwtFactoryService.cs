using App.Data.Entities;
using App.Service.Interfaces;
using App.Service.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading.Tasks;

namespace App.Service.Services
{
    public class JwtFactoryService : IJwtFactoryService
    {
        private readonly JwtIssuerOptions _jwtOptions;
        private readonly UserManager<AppIdentityUser> _userManager;
        private static readonly JwtSecurityTokenHandler tokenHandler = new();


        public JwtFactoryService(IOptions<JwtIssuerOptions> jwtOptions, UserManager<AppIdentityUser> userManager)
        {
            _jwtOptions = jwtOptions.Value;
            _userManager = userManager;

            ThrowIfInvalidOptions(_jwtOptions);
        }
        private static void ThrowIfInvalidOptions(JwtIssuerOptions options)
        {
            if (options == null)
                throw new ArgumentNullException(nameof(options));

            if (options.ValidFor <= TimeSpan.Zero)
            {
                throw new ArgumentException("Must be a non-zero TimeSpan.", nameof(JwtIssuerOptions.ValidFor));
            }

            if (options.JtiGenerator == null)
            {
                throw new ArgumentNullException(nameof(JwtIssuerOptions.JtiGenerator));
            }
        }
        public async Task<string> GenerateEncodedToken(string userName, ClaimsIdentity identity)
        {
            var claims = new[]
         {
                    new Claim(JwtRegisteredClaimNames.Sub, userName),
                    new Claim(JwtRegisteredClaimNames.Jti, await _jwtOptions.JtiGenerator()),
                    new Claim(JwtRegisteredClaimNames.Iat, ToUnixEpochDate(_jwtOptions.IssuedAt).ToString(), ClaimValueTypes.Integer64),
                    identity.FindFirst(Helpers.Constants.Strings.JwtClaimIdentifiers.Rol),
                    identity.FindFirst(Helpers.Constants.Strings.JwtClaimIdentifiers.Id),
                    identity.FindFirst(Helpers.Constants.Strings.JwtClaimIdentifiers.UserName),
                    identity.FindFirst(Helpers.Constants.Strings.JwtClaimIdentifiers.Email),
                    identity.FindFirst(Helpers.Constants.Strings.JwtClaimIdentifiers.FullName)
                };

            // Create the JWT security token and encode it.
            var jwt = new JwtSecurityToken(
                issuer: _jwtOptions.Issuer,
                audience: _jwtOptions.Audience,
                claims: claims,
                notBefore: _jwtOptions.NotBefore,
                expires: _jwtOptions.Expiration,
                signingCredentials: _jwtOptions.SigningCredentials);

            var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);

            return encodedJwt;
        }
        public ClaimsIdentity GenerateClaimsIdentity(AppIdentityUser user)
        {
            return new ClaimsIdentity(new GenericIdentity(user.UserName, "Token"), new[]
            {
                new Claim(Helpers.Constants.Strings.JwtClaimIdentifiers.Email, user.Email),
                new Claim(Helpers.Constants.Strings.JwtClaimIdentifiers.FullName, $"{user.FirstName} {user.LastName}"),
                new Claim(Helpers.Constants.Strings.JwtClaimIdentifiers.Id, user.Id),
                new Claim(Helpers.Constants.Strings.JwtClaimIdentifiers.Rol, Helpers.Constants.Strings.JwtClaims.ApiAccess)
            });
        }
        private static long ToUnixEpochDate(DateTime date)
          => (long)Math.Round((date.ToUniversalTime() -
                               new DateTimeOffset(1970, 1, 1, 0, 0, 0, TimeSpan.Zero)).TotalSeconds);

        public static JwtSecurityToken GetToken(HttpRequest request) => tokenHandler.ReadJwtToken(request.Headers["Authorization"].ToString().Replace("Bearer ", ""));

        public static string GetClaimValue(JwtSecurityToken token, string key) => token.Claims.FirstOrDefault(x => x.Type.Equals(key)).Value;

    }
}
