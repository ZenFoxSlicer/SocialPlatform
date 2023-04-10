using App.Data.Entities;
using App.Service.Models;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Service.Interfaces
{
    public interface IPublicationService
    {
        Task<PaginatedResponse<PublicationModel>> GetList(PaginatedRequest paginatedRequest, JwtSecurityToken jwtSecurityToken);
        Task Upsert(PublicationModel publicationModel, JwtSecurityToken decodedToken);
        Task Delete(string id);
        Task<PaginatedResponse<PublicationExternalModel>> GetPublicationExternalList(PaginatedRequest paginatedRequest,string forUser, JwtSecurityToken jwtSecurityToken);
        Task<PaginatedResponse<PublicationExternalModel>> GetDashboardPublicationList(PaginatedRequest paginatedRequest, JwtSecurityToken jwtSecurityToken);
        Task<CommentModel> PostComment(string publicationId, CommentModel comment, JwtSecurityToken jwtSecurityToken);
        Task DeleteComment(string id, JwtSecurityToken jwtSecurityToken);
        Task<string> LikePost(string id, JwtSecurityToken jwtSecurityToken);
        Task<string> UnlikePost(string id, JwtSecurityToken jwtSecurityToken);
    }
}
