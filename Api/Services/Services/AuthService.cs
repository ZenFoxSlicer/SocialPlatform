using App.Data.Entities;
using App.Service.Interfaces;
using App.Service.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.IdentityModel.Tokens;
using AutoMapper;
using App.Data.Data;
using Microsoft.EntityFrameworkCore;

namespace App.Service.Services
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<AppIdentityUser> userManager;
        private readonly IJwtFactoryService jwtFactory;
        private readonly IConfiguration configuration;
        private readonly IEmailSender emailSender;
        private readonly ApplicationDbContext context;
        private readonly IMapper mapper;
        public AuthService(
            UserManager<AppIdentityUser> userManager,
            IJwtFactoryService jwtFactory,
            IConfiguration configuration,
            IEmailSender emailSender,
            ApplicationDbContext context,
            IMapper mapper

            )
        {
            this.userManager = userManager;
            this.jwtFactory = jwtFactory;
            this.configuration = configuration;
            this.emailSender = emailSender;
            this.context = context;
            this.mapper = mapper;
        }

        public async Task<UserModel> GetBasicUserInfo(string userName)
        {
            var user = await userManager.FindByNameAsync(userName);

            return mapper.Map<UserModel>(user);
        }

        public async Task<LoginResultModel> LogIn(CredentialsModel credentials)
        {
            var identity = await GetClaimsIdentity(credentials.UserName, credentials.Password);
            if (identity == null)
            {
                return null;
            }

            var jwt = await jwtFactory.GenerateEncodedToken(identity.Name, identity);

            return new LoginResultModel
            {
                AuthToken = jwt
            };
        }

        public async Task<IEnumerable<string>> ResetPassword(ResetPasswordModel resetPasswordModel)
        {
            var user = await userManager.FindByEmailAsync(resetPasswordModel.Email);

            if (!resetPasswordModel.Password.Equals(resetPasswordModel.ConfirmPassword))
            {
                throw new InvalidOperationException("Passwords don't match");
            }
            var newPassword = await userManager.ResetPasswordAsync(user, resetPasswordModel.Token, resetPasswordModel.Password);

            return newPassword.Errors.Select(x => x.Description);
        }

        public async Task<LoginResultModel> SaveUserInfo(UserModel model)
        {
            var user = await userManager.FindByIdAsync(model.Id);
            if (user == null)
            {
                return null;
            }

            try
            {
                user.UserName = model.UserName;
                user.FirstName = model.FirstName;
                user.LastName = model.LastName;
                user.Email = model.Email;
                user.PhoneNumber = model.PhoneNumber;

                await userManager.UpdateAsync(user);

                var identity = await GetClaimsIdentity(user.UserName);

                if (identity != null)
                {
                    var jwt = await jwtFactory.GenerateEncodedToken(identity.Name, identity);

                    return new LoginResultModel
                    {
                        AuthToken = jwt
                    };
                }
                return null;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException(ex.Message, ex);
            }


        }

        public async Task SendResetPasswordLink(ForgotPasswordModel forgotPasswordModel)
        {
            var user = await userManager.FindByNameAsync(forgotPasswordModel.UserName);
            var email = await userManager.FindByEmailAsync(forgotPasswordModel.Email);

            if (user == null && email == null)
                throw new ArgumentException("User does not exist");

            if (!user.Email.Equals(forgotPasswordModel.Email) || !email.UserName.Equals(forgotPasswordModel.UserName))
                throw new ArgumentException("User email and username don't match");

            var token = await userManager.GeneratePasswordResetTokenAsync(user);
            var param = new Dictionary<string, string>
                {
                    {"token", token },
                    {"email", forgotPasswordModel.Email }
                };

            var callback = QueryHelpers.AddQueryString(configuration.GetValue<string>("ClientURI"), param);
            try
            {
                await emailSender.SendEmailAsync(user.Email, "Reset password token", callback);
            }
            catch (Exception ex)
            {
                throw new ArgumentException(ex.Message);
            }
        }
        private async Task<ClaimsIdentity> GetClaimsIdentity(string userName)
        {
            // get the user to verifty
            var userByName = await userManager.FindByNameAsync(userName);

            if (userByName != null)
            {
                var claimsIdentity = jwtFactory.GenerateClaimsIdentity(userByName);
                return await Task.FromResult(claimsIdentity);
            }

            // Credentials are invalid, or account doesn't exist
            return await Task.FromResult<ClaimsIdentity>(null);
        }

        private async Task<ClaimsIdentity> GetClaimsIdentity(string login, string password)
        {
            if (!string.IsNullOrEmpty(login) && !string.IsNullOrEmpty(password))
            {
                // get the user to verifty
                var userByName = await userManager.FindByNameAsync(login);
                var userByEmail = await userManager.FindByEmailAsync(login);

                if (userByEmail != null || userByName != null)
                {
                    var user = userByName ?? userByEmail;

                    if (await userManager.CheckPasswordAsync(user, password))
                    {
                        var claimsIdentity = jwtFactory.GenerateClaimsIdentity(user);
                        return await Task.FromResult(claimsIdentity);
                    }
                }
            }

            // Credentials are invalid, or account doesn't exist
            return await Task.FromResult<ClaimsIdentity>(null);
        }
    }
}
