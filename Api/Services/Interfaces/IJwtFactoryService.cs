﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Security.Claims;
using System.Threading.Tasks;

namespace App.Service.Interfaces
{
    public interface IJwtFactoryService
    {
        Task<string> GenerateEncodedToken( string userName , ClaimsIdentity identity );
        ClaimsIdentity GenerateClaimsIdentity( string email , string id );
    }
}
