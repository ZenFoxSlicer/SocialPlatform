﻿using App.Data.Data;
using App.Data.Entities;
using App.Service.Interfaces;
using App.Service.Models;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace App.Api.Controllers
{
    [Route( "api/register" )]
    [ApiController]
    public class RegistrationController : ControllerBase
    {
        private readonly IRegistrationService _registrationService;
        private readonly ApplicationDbContext _appDbContext;
        
        

        public RegistrationController(
            IRegistrationService registrationService,
             
             
            ApplicationDbContext appDbContext )
        {
            _registrationService = registrationService;
            
           
            _appDbContext = appDbContext;
        }

        [HttpPost]
        public async Task<IActionResult> Register( [FromBody]RegistrationViewModel model )
        {
            if ( !ModelState.IsValid )
            {
                return BadRequest( ModelState );
            }

            var result = await _registrationService.Register( model );

            if ( !result.Succeeded )
            {
                ModelState.AddModelError( "Error" , result.ToString() );
                return new BadRequestObjectResult( ModelState );
            }
            return new JsonResult("User registered successfully");
        }
    }
}
