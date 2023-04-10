using App.Service.Models;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Service.Interfaces
{
    public interface IFollowersService
    {
        Task Follow(string id, JwtSecurityToken jwtSecurityToken);
        Task Unfollow(string id, JwtSecurityToken jwtSecurityToken);
        Task<IEnumerable<UserModel>> GetFollowersList(JwtSecurityToken jwtSecurityToken);
        Task<IEnumerable<UserModel>> GetFollowingList(JwtSecurityToken jwtSecurityToken);
        Task<bool> IsFollowing(string id, JwtSecurityToken jwtSecurityToken);
    }
}
