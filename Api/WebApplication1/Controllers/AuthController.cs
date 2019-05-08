using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using App.Service.Models;
using App.Service.Services;
using Microsoft.AspNetCore.Identity;
using App.Service.Interfaces;
using Microsoft.Extensions.Options;
using App.Data.Entities;
using Newtonsoft.Json;
using System.Security.Claims;


namespace App.Controllers
{
    [Route( "api/auth" )]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<AppIdentityUser> _userManager;
        private readonly IJwtFactoryService _jwtFactory;
        private readonly JsonSerializerSettings _serializerSettings;
        private readonly JwtIssuerOptions _jwtOptions;
        public AuthController( UserManager<AppIdentityUser> userManager , IJwtFactoryService jwtFactory , IOptions<JwtIssuerOptions> jwtOptions )
        {
            _userManager = userManager;
            _jwtFactory = jwtFactory;
            _jwtOptions = jwtOptions.Value;

            _serializerSettings = new JsonSerializerSettings
            {
                Formatting = Formatting.Indented
            };
        }
        [HttpPost( "login" )]
        public async Task<IActionResult> Post ( [FromBody] CredentialsModel credentials )
        {
            if ( !ModelState.IsValid )
            {
                return BadRequest( ModelState );
            }

            var identity = await GetClaimsIdentity( credentials.UserName , credentials.Password );
            if ( identity == null )
            {
                ModelState.AddModelError( "login_failure" , "Invalid username or password." );
                return BadRequest( ModelState );
            }

            var response = new
            {
                id = identity.Claims.Single( c => c.Type == "id" ).Value ,
                auth_token = await _jwtFactory.GenerateEncodedToken( credentials.UserName , identity ) ,
                expires_in = ( int )_jwtOptions.ValidFor.TotalSeconds
            };

            var json = JsonConvert.SerializeObject( response , _serializerSettings );
            return new OkObjectResult( json );
           
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
