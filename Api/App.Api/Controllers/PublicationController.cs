using App.Service.Interfaces;
using App.Service.Models;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Http.HttpResults;
using Azure;
using Microsoft.AspNetCore.Http;
using App.Service.Services;
using App.Data.Entities;
using Microsoft.AspNetCore.Authorization;

namespace App.Api.Controllers
{
    //[Authorize]
    [Route("publications")]
    [ApiController]
    [EnableCors]
    public class PublicationController : ControllerBase
    {
        private readonly IPublicationService publicationService;
        public PublicationController(
            IPublicationService publicationService)
        {
            this.publicationService = publicationService;
        }

        [HttpPost("upsert")]
        public async Task<IActionResult> UpsertPublication([FromBody] PublicationModel publicationModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                await publicationService.Upsert(publicationModel, JwtFactoryService.GetToken(Request));
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }



        [HttpPost("post-comment/{publicationId}")]
        public async Task<IActionResult> PostComment([FromBody] CommentModel comment, [FromRoute] string publicationId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var response = await publicationService.PostComment(publicationId, comment, JwtFactoryService.GetToken(Request));

                return new JsonResult(response);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("get-list")]
        public async Task<IActionResult> GetPublicationList([FromBody] PaginatedRequest paginatedRequest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var response = await publicationService.GetList(paginatedRequest, JwtFactoryService.GetToken(Request));

                return new JsonResult(response);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePublication([FromRoute] string id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                await publicationService.Delete(id);

                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("comments/{id}")]
        public async Task<IActionResult> DeleteComment([FromRoute] string id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                await publicationService.DeleteComment(id, JwtFactoryService.GetToken(Request));

                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

    }
}
