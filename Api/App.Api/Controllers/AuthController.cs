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
        private readonly IAuthService _authService;
        public AuthController( IAuthService authService )
        {
            _authService = authService;
        }
        [HttpPost( "login" )]
        public async Task<IActionResult> Login ( [FromBody] CredentialsModel credentials )
        {
            if ( !ModelState.IsValid )
            {
                return BadRequest( ModelState );
            }
            var response = await _authService.LogIn(credentials);

            if ( response == null )
            {
                return BadRequest(new JsonResult("Invalid Login or Password"));
            }
            return new JsonResult( response );
        }
    }
}
