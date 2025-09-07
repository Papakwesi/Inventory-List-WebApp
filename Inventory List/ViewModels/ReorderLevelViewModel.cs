using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Inventory_List.ViewModels
{
    public class ReorderLevelViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Please select a product")]
        public int ProductId { get; set; }

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Level must be greater than 0")]
        public int Level { get; set; }

        [ValidateNever]
        public IEnumerable<SelectListItem> Products { get; set; }
    }

}
