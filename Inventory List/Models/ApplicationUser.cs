using Microsoft.AspNetCore.Identity;

namespace Inventory_List.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string? RoleName { get; set; }
        public string? Description { get; set; }
    }
}
