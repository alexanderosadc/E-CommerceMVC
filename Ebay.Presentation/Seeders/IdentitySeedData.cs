using Ebay.Domain.Entities;
using Ebay.Domain.Interfaces;
using Ebay.Infrastructure.Persistance;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Ebay.Presentation.Seeders
{
    public static class IdentitySeedData
    {
        private const string adminUserName = "Admin";
        private const string adminPassword = "Secret123$";
        private const string adminEmail = "email@email.com";
        private const string adminPhone = "12345678";

        private const string adminRole = "admin";
        private const string userRole = "user";

        public static async void EnsurePopulatedRoles(IApplicationBuilder app)
        {
            AppDbContext context = app.ApplicationServices
                .CreateScope().ServiceProvider
                .GetRequiredService<AppDbContext>();
            if (context.Database.GetPendingMigrations().Any())
            {
                context.Database.Migrate();
            }
            RoleManager<IdentityRole> roleManager = app.ApplicationServices
                .CreateScope().ServiceProvider
                .GetRequiredService<RoleManager<IdentityRole>>();

            IdentityRole adminAppRole = await roleManager.FindByNameAsync(adminRole);
            if(adminAppRole == null)
            {
                adminAppRole = new IdentityRole();
                adminAppRole.Name = adminRole;
                await roleManager.CreateAsync(adminAppRole);
            }

            IdentityRole userAppRole = await roleManager.FindByNameAsync(userRole);
            if (userAppRole == null)
            {
                userAppRole = new IdentityRole();
                userAppRole.Name = userRole;
                await roleManager.CreateAsync(userAppRole);
            }
        }
        public static async void EnsurePopulatedUsers(IApplicationBuilder app)
        {
            AppDbContext context = app.ApplicationServices
                .CreateScope().ServiceProvider
                .GetRequiredService<AppDbContext>();
            if (context.Database.GetPendingMigrations().Any())
            {
                context.Database.Migrate();
            }

            UserManager<User> userManager = app.ApplicationServices
                .CreateScope().ServiceProvider
                .GetRequiredService<UserManager<User>>();
            var user = await userManager.FindByIdAsync(adminUserName);
            if (user == null)
            {
                user = new User();
                user.UserName = adminUserName;
                user.Email = adminEmail;
                user.PhoneNumber = adminPhone;
                await userManager.CreateAsync(user, adminPassword);
            }
            var roleResult = await userManager.AddToRoleAsync(user, adminRole);
        }
    }
}
