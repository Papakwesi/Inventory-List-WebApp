using System.ComponentModel.DataAnnotations;

namespace Inventory_List.Models
{
    public class Category
    {
        public int Id { get; set; }
        [Required] public string Name { get; set; }
        [Required] public string Description { get; set; }
    }
}
