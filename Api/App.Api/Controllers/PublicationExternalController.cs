using App.Service.Models;
using App.Service.Services;
using Azure.Core;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System;
using App.Service.Interfaces;
using Microsoft.IdentityModel.Tokens;

namespace App.Api.Controllers
{
    [Route("publications-external")]
    [ApiController]
    [EnableCors]
    public class PublicationExternalController : ControllerBase
    {
        private readonly IPublicationService publicationService;
        public PublicationExternalController(
            IPublicationService publicationService)
        {
            this.publicationService = publicationService;
        }

        [HttpPost("get-list/{userName}")]
        public async Task<IActionResult> GetPublicationList([FromBody] PaginatedRequest paginatedRequest, [FromRoute] string userName)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var response = await publicationService.GetPublicationExternalList(paginatedRequest, userName, 
                    !Request.Headers["Authorization"].IsNullOrEmpty() ? JwtFactoryService.GetToken(Request) : null);

                return new JsonResult(response);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("get-dashboard-list")]
        public async Task<IActionResult> GetDashboardPublicationList([FromBody] PaginatedRequest paginatedRequest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var response = await publicationService.GetDashboardPublicationList(paginatedRequest, JwtFactoryService.GetToken(Request));

                return new JsonResult(response);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("like/{id}")]
        public async Task<IActionResult> LikePost([FromRoute] string id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var response = await publicationService.LikePost(id, JwtFactoryService.GetToken(Request));

                return new JsonResult(response);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        [HttpGet("unlike/{id}")]
        public async Task<IActionResult> UnlikePost([FromRoute] string id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var response = await publicationService.UnlikePost(id, JwtFactoryService.GetToken(Request));

                return new JsonResult(response);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }
    }
}
