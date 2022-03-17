using Ebay.Domain.Entities;
using Ebay.Infrastructure.Interfaces;
using Ebay.Infrastructure.Persistance;
using Ebay.Infrastructure.ViewModels;
using Ebay.Infrastructure.ViewModels.Admin.Users;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Ebay.Presentation.Business_Logic
{
    public class UserBusinessLogic : IUserBL
    {
        private readonly AppDbContext _context;
        private readonly DbSet<User> _users;
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole> _rolesManager;
        public UserBusinessLogic(AppDbContext context)
        {
            _context = context;
            _users = _context.Set<User>();
        }

        public async Task Delete(int id)
        {
            var user = await _users.FindAsync(id);
            if(user != null)
            {
                await _userManager.DeleteAsync(user);
            }
        }

        public async Task<List<AppUserViewDTO>> GetUsers()
        {
            var allUsers = await _users.ToListAsync();
            var usersDTO = allUsers.Select(async user => await ToUserView(user)).ToList();
            var results = usersDTO.Select(user => user.Result).ToList();
            return results;
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
    }
}
