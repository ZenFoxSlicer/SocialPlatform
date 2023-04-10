using App.Data.Entities;
using App.Service.Interfaces;
using App.Service.Services;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace App.Api.Controllers
{
    //[Authorize]
    [Route("followers")]
    [ApiController]
    [EnableCors]
    public class FollowersController : ControllerBase
    {
        private readonly IFollowersService followersService;
        public FollowersController(
            IFollowersService followersService
            )
        { 
            ArgumentNullException.ThrowIfNull(followersService, nameof(followersService));
            this.followersService = followersService;
        }

        [HttpGet("following/{id}")]
        public async Task<IActionResult> IsFollowing([FromRoute] string id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                var response = await followersService.IsFollowing(id, JwtFactoryService.GetToken(Request));

                return new JsonResult(response);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpGet("follow/{id}")]
        public async Task<IActionResult> Follow([FromRoute] string id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                await followersService.Follow(id, JwtFactoryService.GetToken(Request));

                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("unfollow/{id}")]
        public async Task<IActionResult> Unfollow([FromRoute] string id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                await followersService.Unfollow(id, JwtFactoryService.GetToken(Request));

                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("following-list")]
        public async Task<IActionResult> GetFollowingList([FromRoute] string id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                var response = await followersService.GetFollowingList(JwtFactoryService.GetToken(Request));

                return new JsonResult(response);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("followers-list")]
        public async Task<IActionResult> GetFollowersList([FromRoute] string id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                var response = await followersService.GetFollowersList(JwtFactoryService.GetToken(Request));

                return new JsonResult(response);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
