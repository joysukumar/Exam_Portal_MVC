using Microsoft.AspNetCore.Identity;
using Exam_Portal.Models;

namespace Exam_Portal.Data
{
    public static class SeedRoles_AdminAccounts
    {

        public static async Task SeedRolesAndAdminAsync(IServiceProvider serviceProvider)
        {
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager = serviceProvider.GetRequiredService<UserManager<User>>();

            string[] roles = { "Admin", "User" };
            foreach (var role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                    await roleManager.CreateAsync(new IdentityRole(role));
            }


            var adminUsers = new[]
            {
                new { Email = "admin1@gmail.com", Name = "Admin One", Address = "HQ One", Password = "Admin@123" },
                new { Email = "admin2@gmail.com", Name = "Admin Two", Address = "HQ Two", Password = "Admin@456" },
                new { Email = "admin3@gmail.com", Name = "Admin Three", Address = "HQ Three", Password = "Admin@789" }
            };

            foreach (var admin in adminUsers)
            {
                var existingUser = await userManager.FindByEmailAsync(admin.Email);
                if (existingUser == null)
                {
                    var user = new User
                    {
                        UserName = admin.Email,
                        Email = admin.Email,
                        Name = admin.Name,
                       
                    };

                    var result = await userManager.CreateAsync(user, admin.Password);
                    if (result.Succeeded)
                    {
                        await userManager.AddToRoleAsync(user, "Admin");
                    }
                }
            }

        }
    }
}
