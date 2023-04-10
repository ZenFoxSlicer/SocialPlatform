using App.Data.Data;
using App.Data.Entities;
using App.Service.Interfaces;
using App.Service.Models;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static App.Service.Helpers.Constants.Strings;

namespace App.Service.Services
{
    public class FollowersService : IFollowersService
    {
        private readonly IMapper mapper;
        private readonly ApplicationDbContext context;
        public FollowersService(
            ApplicationDbContext context,
            IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }
        public async Task Follow(string id, JwtSecurityToken jwtSecurityToken)
        {
            var user = await this.context.AspNetUsers.FirstOrDefaultAsync(x => x.Id.Equals(id)) ?? throw new InvalidOperationException("User does not exist");

            var exists = await this.context.Followings.AnyAsync(x => x.FollowedId.Equals(id) && x.FollowerId.Equals(JwtFactoryService.GetClaimValue(jwtSecurityToken, JwtClaimIdentifiers.Id)));

            if (exists)
            {
                return;
            }

            var following = new Following
            {
                FollowedId = user.Id,
                FollowerId = JwtFactoryService.GetClaimValue(jwtSecurityToken, JwtClaimIdentifiers.Id)
            };
            await context.Followings.AddAsync(following);
            await context.SaveChangesAsync();
        }

        public async Task Unfollow(string id, JwtSecurityToken jwtSecurityToken)
        {
            var user = await this.context.AspNetUsers.FirstOrDefaultAsync(x => x.Id.Equals(id)) ?? throw new InvalidOperationException("User does not exist");

            var following = await context.Followings
                .FirstOrDefaultAsync(x => x.FollowedId.Equals(user.Id) && x.FollowerId.Equals(JwtFactoryService.GetClaimValue(jwtSecurityToken, JwtClaimIdentifiers.Id)));

            context.Followings.Remove(following);
            await context.SaveChangesAsync();
        }

        public async Task<bool> IsFollowing(string id, JwtSecurityToken jwtSecurityToken)
        {
            var user = await this.context.AspNetUsers.FirstOrDefaultAsync(x => x.Id.Equals(id)) ?? throw new InvalidOperationException("User does not exist");
            return await context.Followings.AnyAsync(x => x.FollowedId.Equals(user.Id) && x.FollowerId.Equals(JwtFactoryService.GetClaimValue(jwtSecurityToken, JwtClaimIdentifiers.Id)));
        }

        public async Task<IEnumerable<UserModel>> GetFollowersList(JwtSecurityToken jwtSecurityToken)
        {
            var currentUserFollowings = await context.Followings
                .Where(x => x.FollowerId.Equals(JwtFactoryService.GetClaimValue(jwtSecurityToken, JwtClaimIdentifiers.Id)))
                .Select(x => x.FollowedId.ToString())
                .ToListAsync();

            var followers = await context.Followings
                .Where(x => x.FollowedId.Equals(JwtFactoryService.GetClaimValue(jwtSecurityToken, JwtClaimIdentifiers.Id)))
                .Select(x =>
                    new UserModel
                    {
                        Id = x.Follower.Id,
                        FirstName = x.Follower.FirstName,
                        LastName = x.Follower.LastName,
                        UserName = x.Follower.UserName,
                        Email = x.Follower.Email,
                        PhoneNumber = x.Follower.PhoneNumber,
                        CurrentUserIsFollowing = currentUserFollowings.Contains(x.Follower.Id)
                    })
                .ToListAsync();

            return followers;
        }

        public async Task<IEnumerable<UserModel>> GetFollowingList(JwtSecurityToken jwtSecurityToken)
        {
            var following = await context.Followings
                .Where(x => x.FollowerId.Equals(JwtFactoryService.GetClaimValue(jwtSecurityToken, JwtClaimIdentifiers.Id)))
                .Select(x =>
                    new UserModel
                    {
                        Id = x.Followed.Id,
                        FirstName = x.Followed.FirstName,
                        LastName = x.Followed.LastName,
                        UserName = x.Followed.UserName,
                        Email = x.Followed.Email,
                        PhoneNumber = x.Followed.PhoneNumber,
                        CurrentUserIsFollowing = true
                    })
                .ToListAsync();

            return following;
        }


    }
}
