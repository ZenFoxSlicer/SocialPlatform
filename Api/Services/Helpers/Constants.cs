﻿using System;
using System.Collections.Generic;
using System.Text;

namespace App.Service.Helpers
{
    public class Constants
    {
        public static class Strings
        {
            public static class JwtClaimIdentifiers
            {
                public const string Rol = "rol", Id = "id", UserName = "username", Email = "email", FullName = "fullname";
            }

            public static class JwtClaims
            {
                public const string ApiAccess = "api_access";
            }
        }
    }
}
