using App.Data.Entities;
using App.Service.Interfaces;
using App.Service.Models;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
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
        public async Task<IdentityResult> Register(RegistrationViewModel model)
        {
            if (!model.Password.Equals(model.RepeatPassword))
                return IdentityResult.Failed(new IdentityError { Description = "Passwords don't match" });
            var userIdentity = _mapper.Map<AppIdentityUser>(model);
            var result = await _userManager.CreateAsync(userIdentity, model.Password);
            return result;
        }
    }
}
