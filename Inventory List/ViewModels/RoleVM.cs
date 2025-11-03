using System.ComponentModel.DataAnnotations;

namespace Inventory_List.Models.ViewModels
{
    public class RoleVM
    {
        [Required(ErrorMessage = "Role name is required")]
        [Display(Name = "Role Name")]
        public string Name { get; set; }
    }
}
