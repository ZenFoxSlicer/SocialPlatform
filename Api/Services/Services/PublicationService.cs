using App.Data.Data;
using App.Data.Entities;
using App.Service.Interfaces;
using App.Service.Models;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Npgsql.Replication;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;
using static App.Service.Helpers.Constants.Strings;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace App.Service.Services
{
    public class PublicationService : IPublicationService
    {
        private readonly ApplicationDbContext context;
        private readonly IMapper mapper;
        public PublicationService(
            ApplicationDbContext context,
            IMapper mapper
            )
        {
            this.context = context;
            this.mapper = mapper;
        }

        public async Task<PaginatedResponse<PublicationModel>> GetList(PaginatedRequest paginatedRequest, JwtSecurityToken jwtSecurityToken)
        {
            paginatedRequest.SearchString = paginatedRequest.SearchString ?? String.Empty;
            var userId = JwtFactoryService.GetClaimValue(jwtSecurityToken, JwtClaimIdentifiers.Id);

            var query = this.context.Publications
                .Where(x => x.AuthorId.Equals(userId));

            if (!paginatedRequest.SearchString.IsNullOrEmpty())
            {
                query = query.Where(x => x.Title.Contains(paginatedRequest.SearchString)
                    || x.Labels.Contains(paginatedRequest.SearchString));
            }

            var totalItems = query.Count();

            var list = await query
                .OrderByDescending(x => x.DateTime)
                .Skip(paginatedRequest.PageIndex * paginatedRequest.PageSize)
                .Take(paginatedRequest.PageSize)
                .ToListAsync();

            var modelList = mapper.Map<IEnumerable<PublicationModel>>(list);

            return new PaginatedResponse<PublicationModel>
            {
                Data = modelList,
                TotalData = totalItems,
            };
        }

        private static IEnumerable<string> GetLabels(string labels) => labels.Remove(labels.Length, -1).Split(',').ToList();

        public async Task Upsert(PublicationModel publicationModel, JwtSecurityToken decodedToken)
        {
            if (publicationModel.Id.IsNullOrEmpty())
            {
                publicationModel.Id = null;
                var publication = mapper.Map<Publication>(publicationModel);
                publication.AuthorId = JwtFactoryService.GetClaimValue(decodedToken, JwtClaimIdentifiers.Id);
                publication.DateTime = DateTime.UtcNow;

                await context.Publications.AddAsync(publication);
            }
            else
            {
                var existing = await context.Publications
                .FirstOrDefaultAsync(x => x.Id.ToString().Equals(publicationModel.Id)
                                    /*&& x.AuthorId.Equals(JwtFactoryService.GetClaimValue(decodedToken, JwtClaimIdentifiers.Id))*/);
                existing.Title = publicationModel.Title;
                existing.Labels = publicationModel.Labels;
                existing.Body = publicationModel.Body;

                context.Update(existing);
            }

            await context.SaveChangesAsync();
        }

        public async Task Delete(string id)
        {
            var existing = await context.Publications
                .FirstAsync(x => x.Id.ToString().Equals(id)) ?? throw new InvalidOperationException($"Publication with {id} not found");
            context.Publications.Remove(existing);
            await context.SaveChangesAsync();
        }

        public async Task<PaginatedResponse<PublicationExternalModel>> GetPublicationExternalList(PaginatedRequest paginatedRequest, string forUser, JwtSecurityToken jwtSecurityToken)
        {
            var forUserEntity = await this.context.Users.FirstOrDefaultAsync(x => x.UserName == forUser) ?? throw new InvalidOperationException($"User - {forUser}, does not exist");

            var query = context.Publications
                .Where(x => x.AuthorId.Equals(forUserEntity.Id));

            if (!paginatedRequest.SearchString.IsNullOrEmpty())
            {
                query = query.Where(x => x.Title.Contains(paginatedRequest.SearchString)
                    || x.Labels.Contains(paginatedRequest.SearchString));
            }

            var totalItems = query.Count();

            var list = await query
                .OrderByDescending(x => x.DateTime)
                .Skip(paginatedRequest.PageIndex * paginatedRequest.PageSize)
                .Take(paginatedRequest.PageSize)
                .Include(x => x.Comments).ThenInclude(x => x.Author)
                .Include(x => x.Likes).ThenInclude(x => x.Author)
                .ToListAsync();

            var currentUserLikes = await context.Likes
                .Where(x => list.Select(x => x.Id).Contains(x.PublicationId) && x.AuthorId.Equals(JwtFactoryService.GetClaimValue(jwtSecurityToken, JwtClaimIdentifiers.Id)))
                .ToListAsync();

            var modelList = mapper.Map<IEnumerable<PublicationExternalModel>>(list);

            foreach (var userLike in currentUserLikes)
            {
                modelList.First(x => x.Id.Equals(userLike.PublicationId.ToString())).IsLikedByCurrentUser = true;
            }

            return new PaginatedResponse<PublicationExternalModel>
            {
                Data = modelList,
                TotalData = totalItems,
            };
        }

        public async Task<PaginatedResponse<PublicationExternalModel>> GetDashboardPublicationList(PaginatedRequest paginatedRequest, JwtSecurityToken jwtSecurityToken)
        {
            var currentUserFollowings = await context.Followings
                .Where(x => x.FollowerId.Equals(JwtFactoryService.GetClaimValue(jwtSecurityToken, JwtClaimIdentifiers.Id)))
                .Select(x => x.FollowedId.ToString())
                .ToListAsync();

            var query = context.Publications
                .Where(x => currentUserFollowings.Contains(x.AuthorId) || x.AuthorId.Equals(JwtFactoryService.GetClaimValue(jwtSecurityToken, JwtClaimIdentifiers.Id)));

            var totalItems = query.Count();

            var list = await query
                .OrderByDescending(x => x.DateTime)
                .Skip(paginatedRequest.PageIndex * paginatedRequest.PageSize)
                .Take(paginatedRequest.PageSize)
                .Include(x=>x.Author)
                .Include(x => x.Comments).ThenInclude(x => x.Author)
                .Include(x => x.Likes).ThenInclude(x => x.Author)
                .ToListAsync();

            var currentUserLikes = await context.Likes
                .Where(x => list.Select(x => x.Id).Contains(x.PublicationId) && x.AuthorId.Equals(JwtFactoryService.GetClaimValue(jwtSecurityToken, JwtClaimIdentifiers.Id)))
                .ToListAsync();

            var modelList = mapper.Map<IEnumerable<PublicationExternalModel>>(list);

            foreach (var userLike in currentUserLikes)
            {
                modelList.First(x => x.Id.Equals(userLike.PublicationId.ToString())).IsLikedByCurrentUser = true;
            }

            return new PaginatedResponse<PublicationExternalModel>
            {
                Data = modelList,
                TotalData = totalItems,
            };
        }

        public async Task<CommentModel> PostComment(string publicationId, CommentModel commentModel, JwtSecurityToken decodedToken)
        {
            var publication = await context.Publications.FirstOrDefaultAsync(x => x.Id.ToString().Equals(publicationId));
            if (publication == null)
            {
                throw new InvalidOperationException($"Publication with ID - {publication}, does not exist");
            }

            var comment = new Comment
            {
                AuthorId = JwtFactoryService.GetClaimValue(decodedToken, JwtClaimIdentifiers.Id),
                DateTime = DateTime.UtcNow,
                Body = commentModel.Body,
                PublicationId = Guid.Parse(publicationId)
            };

            var result = await context.Comments.AddAsync(comment);
            await context.SaveChangesAsync();

            var newComment = await context.Comments
                .Include(x => x.Author)
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id.Equals(result.Entity.Id));

            return mapper.Map<CommentModel>(newComment);
        }

        public async Task DeleteComment(string id, JwtSecurityToken jwtSecurityToken)
        {
            var currentUser = JwtFactoryService.GetClaimValue(jwtSecurityToken, JwtClaimIdentifiers.Id);
            var comment = await context.Comments
                .Include(x => x.Publication)
                .FirstOrDefaultAsync(x => x.Id.ToString().Equals(id));
            if (!comment.Publication.AuthorId.Equals(currentUser) && !comment.AuthorId.Equals(currentUser))
            {
                throw new InvalidOperationException("Comment does not exist or you have no rights to remove it");
            }
            context.Comments.Remove(comment);
            await context.SaveChangesAsync();
        }

        public async Task<string> LikePost(string id, JwtSecurityToken jwtSecurityToken)
        {
            var publication = await context.Publications.FirstOrDefaultAsync(x => x.Id.ToString().Equals(id));
            if (publication == null)
            {
                throw new InvalidOperationException($"Publication with ID - {publication}, does not exist");
            }

            if (await context.Likes.AnyAsync(x => x.PublicationId.Equals(id) && x.AuthorId.Equals(JwtFactoryService.GetClaimValue(jwtSecurityToken, JwtClaimIdentifiers.Id))))
            {
                throw new InvalidOperationException("Can't like the same post twice");
            }

            var like = new Like
            {
                AuthorId = JwtFactoryService.GetClaimValue(jwtSecurityToken, JwtClaimIdentifiers.Id),
                PublicationId = new Guid(id),
            };

            await context.Likes.AddAsync(like);
            await context.SaveChangesAsync();

            return await context.AspNetUsers
                .Where(x => x.Id.Equals(JwtFactoryService.GetClaimValue(jwtSecurityToken, JwtClaimIdentifiers.Id)))
                .Select(x => $"{x.FirstName} {x.LastName}")
                .FirstOrDefaultAsync();
        }

        public async Task<string> UnlikePost(string id, JwtSecurityToken jwtSecurityToken)
        {
            var publication = await context.Publications.FirstOrDefaultAsync(x => x.Id.ToString().Equals(id));
            if (publication == null)
            {
                throw new InvalidOperationException($"Publication with ID - {publication}, does not exist");
            }

            var existing = await context.Likes.FirstOrDefaultAsync(x => x.PublicationId.ToString().Equals(id) && x.AuthorId.Equals(JwtFactoryService.GetClaimValue(jwtSecurityToken, JwtClaimIdentifiers.Id)));
            if (existing == null)
            {
                throw new InvalidOperationException("Cannot unlike a post tha was not liked in the first place");
            }

            context.Remove(existing);
            await context.SaveChangesAsync();

            return await context.AspNetUsers
                .Where(x => x.Id.Equals(JwtFactoryService.GetClaimValue(jwtSecurityToken, JwtClaimIdentifiers.Id)))
                .Select(x => $"{x.FirstName} {x.LastName}")
                .FirstOrDefaultAsync();
        }

       
    }
}
