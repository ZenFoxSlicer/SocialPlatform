using App.Data.Entities;
using App.Service.Interfaces;
using App.Service.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace App.Service.Services
{
    public class JwtFactoryService : IJwtFactoryService
    {
        private readonly JwtIssuerOptions _jwtOptions;
        private readonly UserManager<AppIdentityUser> _userManager;

        public JwtFactoryService( IOptions<JwtIssuerOptions> jwtOptions , UserManager<AppIdentityUser> userManager )
        {
            _jwtOptions = jwtOptions.Value;
            _userManager = userManager;

            ThrowIfInvalidOptions( _jwtOptions );
        }
        private static void ThrowIfInvalidOptions( JwtIssuerOptions options )
        {
            if ( options == null )
                throw new ArgumentNullException( nameof( options ) );

            if ( options.ValidFor <= TimeSpan.Zero )
            {
                throw new ArgumentException( "Must be a non-zero TimeSpan." , nameof( JwtIssuerOptions.ValidFor ) );
            }

            if ( options.SigningCredentials == null )
            {
                throw new ArgumentNullException( nameof( JwtIssuerOptions.SigningCredentials ) );
            }

            if ( options.JtiGenerator == null )
            {
                throw new ArgumentNullException( nameof( JwtIssuerOptions.JtiGenerator ) );
            }
        }
        public async Task<string> GenerateEncodedToken( string userName , ClaimsIdentity identity )
        {
            var claims = new[]
         {
                 new Claim(JwtRegisteredClaimNames.Sub, userName),
                 new Claim(JwtRegisteredClaimNames.Jti, await _jwtOptions.JtiGenerator()),
                 new Claim(JwtRegisteredClaimNames.Iat, ToUnixEpochDate(_jwtOptions.IssuedAt).ToString(), ClaimValueTypes.Integer64),
                 identity.FindFirst(Helpers.Constants.Strings.JwtClaimIdentifiers.Rol),
                 identity.FindFirst(Helpers.Constants.Strings.JwtClaimIdentifiers.Id)
             };

            // Create the JWT security token and encode it.
            var jwt = new JwtSecurityToken(
                issuer: _jwtOptions.Issuer ,
                audience: _jwtOptions.Audience ,
                claims: claims ,
                notBefore: _jwtOptions.NotBefore ,
                expires: _jwtOptions.Expiration ,
                signingCredentials: _jwtOptions.SigningCredentials );

            var encodedJwt = new JwtSecurityTokenHandler().WriteToken( jwt );

            return encodedJwt;
        }
        public ClaimsIdentity GenerateClaimsIdentity( string userName , string id )
        {
            return new ClaimsIdentity( new GenericIdentity( userName , "Token" ) , new[]
            {
                new Claim(Helpers.Constants.Strings.JwtClaimIdentifiers.Id, id),
                new Claim(Helpers.Constants.Strings.JwtClaimIdentifiers.Rol, Helpers.Constants.Strings.JwtClaims.ApiAccess)
            } );
        }
        private static long ToUnixEpochDate( DateTime date )
          => ( long )Math.Round( ( date.ToUniversalTime() -
                               new DateTimeOffset( 1970 , 1 , 1 , 0 , 0 , 0 , TimeSpan.Zero ) )
                              .TotalSeconds );
    }
}
