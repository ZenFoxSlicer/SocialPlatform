using App.Service.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace App.Service.Interfaces
{
    public interface IAuthService
    {
        Task<UserModel> GetBasicUserInfo(string userName);
        Task<LoginResultModel> LogIn( CredentialsModel credentials );
        Task<IEnumerable<string>> ResetPassword(ResetPasswordModel credentials);
        Task<LoginResultModel> SaveUserInfo(UserModel model);
        Task SendResetPasswordLink(ForgotPasswordModel resetPasswordModel);
    }
}
