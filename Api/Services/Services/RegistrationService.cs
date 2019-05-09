using App.Data.Entities;
using App.Service.Interfaces;
using App.Service.Models;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace App.Service.Services
{
    
    public class RegistrationService : IRegistrationService
    {
        private readonly IMapper _mapper;
        private readonly UserManager<AppIdentityUser> _userManager;
        public RegistrationService(
            IMapper mapper,
            UserManager<AppIdentityUser> userManager
            )
        {
            _mapper = mapper;
            _userManager = userManager;
        }
        public async Task<IdentityResult> Register( RegistrationViewModel model )
        {
            var userIdentity = _mapper.Map<AppIdentityUser>( model );
            var result = await _userManager.CreateAsync( userIdentity , model.Password );
            return result;
        }
    }
}
