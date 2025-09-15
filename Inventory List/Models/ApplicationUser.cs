using Microsoft.AspNetCore.Identity;

namespace Inventory_List.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}
