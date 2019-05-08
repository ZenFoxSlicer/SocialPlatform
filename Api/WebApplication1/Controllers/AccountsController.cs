using App.Data.Data;
using App.Data.Entities;
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
    public class AccountsController : ControllerBase
    {
        private readonly ApplicationDbContext _appDbContext;
        private readonly UserManager<AppIdentityUser> _userManager;
        private readonly IMapper _mapper;

        public AccountsController( 
            UserManager<AppIdentityUser> userManager ,
            IMapper mapper , 
            ApplicationDbContext appDbContext )
        {
            _userManager = userManager;
            _mapper = mapper;
            _appDbContext = appDbContext;
        }

        [HttpPost]
        public async Task<IActionResult> Post( [FromBody]RegistrationViewModel model )
        {
            var i = 1;
            if ( !ModelState.IsValid )
            {
                return BadRequest( ModelState );
            }

            var userIdentity = _mapper.Map<AppIdentityUser>( model );
            var result = await _userManager.CreateAsync( userIdentity , model.Password );

            if ( !result.Succeeded )
            {
                ModelState.AddModelError( "Error" , result.ToString() );
                return new BadRequestObjectResult( ModelState );
            }

            await _appDbContext.TestTable.AddAsync( new TestEntity { field2 = model.Age , field1 = model.Password } );
            await _appDbContext.SaveChangesAsync();

            return new OkResult();
        }
    }
}
