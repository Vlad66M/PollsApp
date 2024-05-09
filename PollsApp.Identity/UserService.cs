using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PollsApp.Application.DTOs;
using PollsApp.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PollsApp.Identity
{
    public class UserService : IUserService
    {
        private readonly UserManager<User> _userManager;

        public UserService(UserManager<User> userManager)
        {
            _userManager = userManager;
        }

        public async Task<List<UserInfo>> GetAllUsers()
        {
            var users = _userManager.Users.ToList();
            List<UserInfo> result = new List<UserInfo>();
            foreach (var user in users)
            {
                UserInfo userDto = new UserInfo();
                userDto.UserId = user.Id;
                userDto.UserName = user.Name;
                userDto.UserAvatar = user.Avatar;
                userDto.UserEmail = user.Email;
                userDto.RoleName = user.Role.Name;
                result.Add(userDto);
            }
            return result;
        }

        public async Task<UserInfo> GetUserInfoById(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            return new UserInfo
            {
                UserEmail = user.Email,
                UserId = user.Id,
                UserName = user.Name,
                RoleName = user.Role.Name,
                UserAvatar = user.Avatar
            };
        }

        public async Task<User> GetUserById(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            return new User
            {
                Email = user.Email,
                Id = user.Id,
                UserName = user.Name,
                Role = new Role() { Name = user.Role.Name },
                Avatar = user.Avatar
            };
        }

    }
}
