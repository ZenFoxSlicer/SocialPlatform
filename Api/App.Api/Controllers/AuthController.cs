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
using Microsoft.AspNetCore.Cors;
using Azure;

namespace App.Controllers
{
    [Route("auth")]
    [ApiController]
    [EnableCors]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService authService;
        public AuthController(IAuthService authService)
        {
            this.authService = authService;
        }
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] CredentialsModel credentials)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var response = await authService.LogIn(credentials);

            if (response == null)
            {
                return BadRequest(new JsonResult("Invalid Login or Password"));
            }

            return new JsonResult(response);
        }

        [HttpPost("forgot-password")]
        public async Task<IActionResult> SendResetPasswordLink([FromBody] ForgotPasswordModel credentials)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                await authService.SendResetPasswordLink(credentials);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordModel credentials)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var errors = await authService.ResetPassword(credentials);

            if (errors.Any())
            {
                return BadRequest(errors);
            }

            return Ok();
        }

        [HttpGet("basic-user-info/{userName}")]
        public async Task<IActionResult> GetBasicUserInfo([FromRoute] string userName)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var results = await authService.GetBasicUserInfo(userName);
                return new JsonResult(results);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("save-user-info")]
        public async Task<IActionResult> SaveUserInfo([FromBody] UserModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var result = await authService.SaveUserInfo(model);
                return new JsonResult(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
