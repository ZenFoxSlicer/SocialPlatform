using System;
using System.Collections.Generic;
using System.Text;
using System.Security.Claims;
using System.Threading.Tasks;
using App.Data.Entities;

namespace App.Service.Interfaces
{
    public interface IJwtFactoryService
    {
        Task<string> GenerateEncodedToken( string userName , ClaimsIdentity identity );
        ClaimsIdentity GenerateClaimsIdentity( AppIdentityUser user );
    }
}
