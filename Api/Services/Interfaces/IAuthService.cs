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
        Task<LoginResultModel> LogIn( CredentialsModel credentials );
    }
}
