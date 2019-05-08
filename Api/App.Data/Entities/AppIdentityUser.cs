using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace App.Data.Entities
{
        public class AppIdentityUser : IdentityUser
        {
            public string FirstName { get; set; }
            public string LastName { get; set; }
        }
}
