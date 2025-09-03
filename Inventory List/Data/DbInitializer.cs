using Inventory_List.Models;
using Microsoft.AspNetCore.Identity;

namespace Inventory_List.Data
{
    public class DbInitializer
    {
        public static async Task SeedAsync(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            // 1. Define roles
            string[] roleNames = { "Admin", "Cashier", "Manager" };

            foreach (var role in roleNames)
            {
                if (!await roleManager.RoleExistsAsync(role))
                {
                    await roleManager.CreateAsync(new IdentityRole(role));
                }
            }

            // 2. Create default Admin user if not exists
            var adminUser = await userManager.FindByEmailAsync("admin@inventory.com");

            if (adminUser == null)
            {
                adminUser = new ApplicationUser
                {
                    UserName = "admin@inventory.com",
                    Email = "admin@inventory.com",
                    EmailConfirmed = true,
                    RoleName = "Admin"
                };

                var result = await userManager.CreateAsync(adminUser, "Admin@123");

                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(adminUser, "Admin");
                }
            }
        }
    }
}
