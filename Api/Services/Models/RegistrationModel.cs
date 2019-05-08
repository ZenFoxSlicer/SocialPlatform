﻿using App.Service.Models.Validators;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using FluentValidation.Attributes;

namespace App.Service.Models
{
    
        [Validator ( typeof( RegistrationViewModelValidator ) )]
        public class RegistrationViewModel 
        {
            public string Email { get; set; }
            public string Password { get; set; }
            public string FirstName { get; set; }
            public string LastName { get; set; }
            public string UserName { get; set; }
            public long Age { get; set; }
        }
}
