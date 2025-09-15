using Inventory_List.Models;
using Microsoft.AspNetCore.Identity;

namespace Inventory_List.Data
{
    public class DbInitializer
    {
        public static async Task SeedAsync(
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager)
        {
            // 1. Define roles
            string[] roleNames = { "SuperAdmin", "Admin", "Employee" };

            foreach (var role in roleNames)
            {
                if (!await roleManager.RoleExistsAsync(role))
                {
                    await roleManager.CreateAsync(new IdentityRole(role));
                }
            }
        }
    }
}
