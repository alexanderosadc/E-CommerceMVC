using Ebay.Domain.Entities;
using Ebay.Infrastructure.Interfaces;
using Ebay.Infrastructure.Persistance;
using Ebay.Infrastructure.ViewModels;
using Ebay.Infrastructure.ViewModels.Admin.Users;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace Ebay.Presentation.Business_Logic
{
    public class UserBusinessLogic : IUserBL
    {
        private readonly UserManager<User> _userManager;
        public UserBusinessLogic(
            UserManager<User> userManager)
        {
            _userManager = userManager;
        }

        public async Task Delete(string id)
        {
            
            var user = await _userManager.FindByIdAsync(id);
            if(user != null)
            {
                await _userManager.DeleteAsync(user);
            }
        }

        public async Task<List<AppUserViewDTO>> GetUsers()
        {
            var allUsers = await _userManager.Users.ToListAsync();

            List<AppUserViewDTO> appUserViewDTOs = new List<AppUserViewDTO>();
            foreach (var user in allUsers)
            {
                var userView = await ToUserView(user);
                if(!userView.UserRoles.Contains("admin"))
                {
                    appUserViewDTOs.Add(userView);
                }
            }
            /*var usersDTO = allUsers.Select(async user => await ToUserView(user)).ToList();
            var results = usersDTO.Select(user => user.Result).ToList();*/
            return appUserViewDTOs;
        }

        public async Task<AppUserViewDTO> ToUserView(User user)
        {
            var roles = await _userManager.GetRolesAsync(user);
            return new AppUserViewDTO
            {
                Id = user.Id,
                UserName = user.UserName,
                Email = user.Email,
                UserRoles = roles
            };
        }
        public async Task<AppUserCreateDTO> GetUserDTO(string itemId)
        {
            var user = await _userManager.FindByIdAsync(itemId);
            var roles = await _userManager.GetRolesAsync(user);
            bool isModerator = false;
            if (roles.Contains("moderator"))
            {
                isModerator = true;
            }

            return new AppUserCreateDTO
            {
                Id = user.Id,
                UserName = user.UserName,
                Email = user.Email,
                IsModerator = isModerator
            };
        }
        public async Task CreateUser(AppUserCreateDTO dto)
        {
            var role = "user";
            if(dto.Password == dto.ConfirmedPassword)
            {
                var user = new User
                {
                    UserName = dto.UserName,
                    Email = dto.Email,
                };

                if(dto.IsModerator)
                {
                    role = "moderator";
                }

                await _userManager.CreateAsync(user, dto.Password);
                await _userManager.AddToRoleAsync(user, role);
            }
        }

        public async Task EditUser(bool isModerator, string userId)
        {
            string role = "user";
            var user = await _userManager.FindByIdAsync(userId);
            List<string> rolesToRemove = new List<string> {"moderator", "user"};

            if(isModerator)
            {
                role = "moderator";
            }
            await _userManager.RemoveFromRolesAsync(user, rolesToRemove);
            await _userManager.AddToRoleAsync(user, role);
        }

       /* public async Task<bool> IsUserExist(string username)
        {
            var user = await _userManager.FindByNameAsync(username);
            return (user != null) ? true : false;
        }*/
    }
}