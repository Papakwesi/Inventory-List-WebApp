using Inventory_List.Models;
using System.ComponentModel.DataAnnotations;

namespace Inventory_List.Models
{
    public class ReorderLevel
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "Product")]
        public int ProductId { get; set; }
        public Product Product { get; set; }

        [Required]
        [Range(0, 10000, ErrorMessage = "Reorder level must be at least 0")]
        [Display(Name = "Reorder Level")]
        public int Level { get; set; }
    }
}
