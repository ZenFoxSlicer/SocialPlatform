using App.Data.Entities;
using App.Service.Interfaces;
using App.Service.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace App.Service.Services
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<AppIdentityUser> _userManager;
        private readonly IJwtFactoryService _jwtFactory;
        public AuthService(
            UserManager<AppIdentityUser> userManager,
            IJwtFactoryService jwtFactory
            )
        {
            _userManager = userManager;
            _jwtFactory = jwtFactory;
            
        }
        public async Task<LoginResultModel> LogIn( CredentialsModel credentials )
        {
            var identity = await GetClaimsIdentity( credentials.UserName , credentials.Password );
            if ( identity == null )
            {
                return null;
            }

            var jwt = await _jwtFactory.GenerateEncodedToken( credentials.UserName , identity );

            return new LoginResultModel 
            {
                AuthToken = jwt
            };
            
        }
        private async Task<ClaimsIdentity> GetClaimsIdentity( string userName , string password )
        {
            if ( !string.IsNullOrEmpty( userName ) && !string.IsNullOrEmpty( password ) )
            {
                // get the user to verifty
                var userToVerify = await _userManager.FindByNameAsync( userName );

                if ( userToVerify != null )
                {
                    // check the credentials  
                    if ( await _userManager.CheckPasswordAsync( userToVerify , password ) )
                    {
                        return await Task.FromResult( _jwtFactory.GenerateClaimsIdentity( userName , userToVerify.Id ) );
                    }
                }
            }

            // Credentials are invalid, or account doesn't exist
            return await Task.FromResult<ClaimsIdentity>( null );
        }
    }
}
