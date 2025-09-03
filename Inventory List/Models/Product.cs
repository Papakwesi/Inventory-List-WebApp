using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;

namespace Inventory_List.Models
{
    public class Product
    {
        [Key] 
        public int Id { get; set; }
        [Required] 
        public string Name { get; set; }
        public string? Code { get; set; }
        [Required] 
        public int Quantity { get; set; }
        [Required] 
        public int UnitPrice { get; set; }
        [Required]
        [Display(Name = "Category")]
        public int CategoryId { get; set; }
        [ValidateNever]
        public Category Category { get; set; }
    }
}
